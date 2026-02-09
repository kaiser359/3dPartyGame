using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardsVote : MonoBehaviour
{
    [Header("Prefabs & UI")]
    public LevelUpCard cardPrefab;
    public Transform cardsParent; // parent transform for instantiated cards (should be a UI container)
    public TextMeshProUGUI hai;

    [Header("Vote options (one per card)")]
    public string[] sceneNames;
    public Sprite[] sceneIcons; // optional, may be shorter than sceneNames

    [Header("Timing & behavior")]
    public float voteDuration = 20f;
    public bool autoLoadWinningScene = true;

    // runtime
    private List<LevelUpCard> cardInstances = new List<LevelUpCard>();
    private static int[] votes;
    private static Dictionary<int, int> playerVotes = new Dictionary<int, int>(); // key = playerIndex, value = optionIndex
    private bool isVotingActive = false;
    private Coroutine timerCoroutine;

    // mapping local option index -> index in sceneNames
    private int[] optionSceneIndices = new int[0];

    // Event: scene name of the winning option
    public Action<string> OnVoteComplete;

    // Start voting. Only runs when called.
    public void StartVoting()
    {
        if (isVotingActive) return;
        if (cardPrefab == null || cardsParent == null || sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogError("CardsVote: Missing setup (cardPrefab, cardsParent or sceneNames).");
            return;
        }

        // prepare
        isVotingActive = true;
        playerVotes.Clear();

        // Choose exactly up to 2 unique random options from sceneNames
        int optionCount = Math.Min(2, sceneNames.Length);
        var pool = Enumerable.Range(0, sceneNames.Length).ToList();
        optionSceneIndices = new int[optionCount];
        for (int i = 0; i < optionCount; i++)
        {
            int pick = UnityEngine.Random.Range(0, pool.Count);
            optionSceneIndices[i] = pool[pick];
            pool.RemoveAt(pick);
        }

        // Random chance to swap which of the two chosen scenes appears as left/right
        if (optionCount == 2 && UnityEngine.Random.value < 0.5f)
        {
            int tmp = optionSceneIndices[0];
            optionSceneIndices[0] = optionSceneIndices[1];
            optionSceneIndices[1] = tmp;
        }

        votes = new int[optionCount];

        // create UI cards (only for the selected two)
        ClearExistingCards();
        for (int i = 0; i < optionCount; i++)
        {
            // Instantiate without parent first to avoid "parent is persistent" warning,
            // then explicitly set parent (keeps correct hierarchy and UI layout).
            var inst = Instantiate(cardPrefab);
            inst.transform.SetParent(cardsParent, false);

            int sceneIndex = optionSceneIndices[i];
            var title = sceneNames[sceneIndex];
            var icon = (sceneIcons != null && sceneIndex < sceneIcons.Length) ? sceneIcons[sceneIndex] : null;
            int idx = i; // local copy for lambda

            // Buttons are no longer used for voting. Do not wire up click callbacks.
            // Keep Setup to populate UI, but pass null as the click handler.
            inst.Setup(title, "", icon, 0, null);
            cardInstances.Add(inst);
        }

        // Do not force-select any button via MultiplayerEventSystem since voting uses input actions now.

        // subscribe to all PlayerInput players
        SubscribeToPlayerInputs();

        // start timer
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(VoteTimerRoutine(voteDuration));
    }

    public void StopVoting()
    {
        if (!isVotingActive) return;
        isVotingActive = false;
        UnsubscribeFromPlayerInputs();
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = null;
        ClearExistingCards();
    }

    // Public method to register a vote from anywhere (e.g., player controllers)
    // playerIndex should be the PlayerInput.playerIndex for that player
    public void RegisterVote(int playerIndex, int optionIndex)
    {
        if (!isVotingActive) return;
        if (optionIndex < 0 || optionIndex >= votes.Length) return;

        // ensure one vote per player
        if (playerVotes.ContainsKey(playerIndex))
        {
            Debug.Log($"Player {playerIndex} already voted. Ignoring subsequent vote.");
            return;
        }

        playerVotes[playerIndex] = optionIndex;
        votes[optionIndex]++;
        UpdateCardVotesDisplay();

    }

    private IEnumerator VoteTimerRoutine(float seconds)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            yield return null;
            if (hai != null) hai.text = $"Voting ends in {Mathf.CeilToInt(remaining)} seconds";
            remaining -= Time.deltaTime;

        }
        FinishVoting();
    }

    private void FinishVoting()
    {
        isVotingActive = false;
        UnsubscribeFromPlayerInputs();

        // determine winner (highest votes). On tie, choose randomly among tied options.
        int max = votes.Max();
        List<int> winners = new List<int>();
        for (int i = 0; i < votes.Length; i++)
            if (votes[i] == max) winners.Add(i);

        int winningLocalIndex = winners.Count > 1 ? winners[UnityEngine.Random.Range(0, winners.Count)] : winners[0];

        // Map back to the original sceneNames index
        string winningScene = sceneNames[optionSceneIndices[winningLocalIndex]];
        Debug.Log($"Voting finished. Winning scene: {winningScene} (local option {winningLocalIndex}, sceneIndex {optionSceneIndices[winningLocalIndex]}), votes: {votes[winningLocalIndex]}");

        OnVoteComplete?.Invoke(winningScene);

        if (autoLoadWinningScene)
        {
            // safe check: ensure the scene is in build settings or loaded by name
            try
            {
                SceneManager.LoadScene(winningScene);
            }
            catch (Exception ex)
            {
                Debug.LogError($"CardsVote: Failed to load scene '{winningScene}'. Exception: {ex.Message}");
            }
        }

        // cleanup UI if desired
        // ClearExistingCards();
    }

    private void UpdateCardVotesDisplay()
    {
        for (int i = 0; i < cardInstances.Count; i++)
        {
            if (i < votes.Length)
                cardInstances[i].SetVotes(votes[i]);
        }
    }

    private void ClearExistingCards()
    {
        foreach (var c in cardInstances)
        {
            if (c != null) Destroy(c.gameObject);
        }
        cardInstances.Clear();
    }

    // Subscribe to actions on each PlayerInput to allow each player to vote.
    // New behavior: look for "attack" (vote left) and "crouch" (vote right) actions in each PlayerInput.
    private List<(PlayerInput player, List<InputActionReferenceBinding>)> subscriptions = new List<(PlayerInput, List<InputActionReferenceBinding>)>();

    private void SubscribeToPlayerInputs()
    {
        UnsubscribeFromPlayerInputs();
        foreach (var p in PlayerInput.all)
        {
            var bound = new List<InputActionReferenceBinding>();

            bool foundAttackOrCrouch = false;

            // ATTACK -> left card (index 0 or fallback)
            var attackAction = p.actions.FindAction("attack", false);
            if (attackAction != null)
            {
                foundAttackOrCrouch = true;
                int playerIndex = p.playerIndex;
                System.Action<InputAction.CallbackContext> attackCb = ctx =>
                {
                    int leftIndex = GetLeftOptionIndex();
                    RegisterVote(playerIndex, leftIndex);
                };
                attackAction.performed += attackCb;
                bound.Add(new InputActionReferenceBinding { action = attackAction, callback = attackCb });
            }

            // CROUCH -> right card (index 1 or fallback)
            var crouchAction = p.actions.FindAction("crouch", false);
            if (crouchAction != null)
            {
                foundAttackOrCrouch = true;
                int playerIndex = p.playerIndex;
                System.Action<InputAction.CallbackContext> crouchCb = ctx =>
                {
                    int rightIndex = GetRightOptionIndex();
                    RegisterVote(playerIndex, rightIndex);
                };
                crouchAction.performed += crouchCb;
                bound.Add(new InputActionReferenceBinding { action = crouchAction, callback = crouchCb });
            }

            if (!foundAttackOrCrouch)
            {
                // fallback: try per-option actions: VoteOption0, VoteOption1, ...
                bool anyBound = false;
                for (int opt = 0; opt < votes.Length; opt++)
                {
                    var name = $"VoteOption{opt}";
                    var action = p.actions.FindAction(name, false);
                    if (action != null)
                    {
                        anyBound = true;
                        int playerIndex = p.playerIndex;
                        int optIndex = opt;
                        System.Action<InputAction.CallbackContext> cb = ctx => RegisterVote(playerIndex, optIndex);
                        action.performed += cb;
                        bound.Add(new InputActionReferenceBinding { action = action, callback = cb });
                    }
                }

                if (!anyBound)
                {
                    // fallback: try a single "Vote" action that returns an int
                    var fallback = p.actions.FindAction("Vote", false);
                    if (fallback != null)
                    {
                        int playerIndex = p.playerIndex;
                        System.Action<InputAction.CallbackContext> fb = ctx =>
                        {
                            int selected = 0;
                            try
                            {
                                selected = ctx.ReadValue<int>();
                            }
                            catch
                            {
                                // if value can't be read as int, ignore
                                return;
                            }
                            RegisterVote(playerIndex, selected);
                        };
                        fallback.performed += fb;
                        bound.Add(new InputActionReferenceBinding { action = fallback, callback = fb });
                    }
                }
            }

            // keep track so we can unsubscribe
            if (bound.Count > 0)
                subscriptions.Add((p, bound));
        }
    }

    private void UnsubscribeFromPlayerInputs()
    {
        foreach (var entry in subscriptions)
        {
            var p = entry.player;
            foreach (var b in entry.Item2)
            {
                if (b.action != null && b.callback != null)
                {
                    b.action.performed -= b.callback;
                }
            }
        }
        subscriptions.Clear();
    }

    // helper type to keep track of what we subscribed to
    private class InputActionReferenceBinding
    {
        public UnityEngine.InputSystem.InputAction action;
        public System.Action<InputAction.CallbackContext> callback;
    }

    private int GetLeftOptionIndex()
    {
        // left maps to index 0 if available, otherwise clamp
        if (votes == null || votes.Length == 0) return 0;
        return 0;
    }

    private int GetRightOptionIndex()
    {
        // right maps to index 1 if available, otherwise use last option (or 0)
        if (votes == null || votes.Length == 0) return 0;
        return votes.Length > 1 ? 1 : votes.Length - 1;
    }

}
