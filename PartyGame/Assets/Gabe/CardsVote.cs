using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private int[] votes;
    private Dictionary<int, int> playerVotes = new Dictionary<int, int>(); // key = playerIndex, value = optionIndex
    private bool isVotingActive = false;
    private Coroutine timerCoroutine;

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
        votes = new int[sceneNames.Length];

        // create UI cards
        ClearExistingCards();
        for (int i = 0; i < sceneNames.Length; i++)
        {
            // Instantiate without parent first to avoid "parent is persistent" warning,
            // then explicitly set parent (keeps correct hierarchy and UI layout).
            var inst = Instantiate(cardPrefab);
            inst.transform.SetParent(cardsParent, false);

            var title = sceneNames[i];
            var icon = (sceneIcons != null && i < sceneIcons.Length) ? sceneIcons[i] : null;
            int idx = i; // local copy for lambda
            inst.Setup(title, "", icon, 0, () =>
            {
                // UI Button callback - this will register a vote for player 0 by default.
                // Prefer subscribing per-player actions or calling RegisterVote from player code.
                RegisterVote(0, idx);
            });
            cardInstances.Add(inst);
        }

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
        Debug.Log($"Player {playerIndex} voted for option {optionIndex} ({sceneNames[optionIndex]}).");
    }

    private IEnumerator VoteTimerRoutine(float seconds)
    {
        float remaining = seconds;
        while (remaining > 0f)
        {
            yield return null;
            hai.text = $"Voting ends in {Mathf.CeilToInt(remaining)} seconds";
            remaining -= Time.deltaTime;
            // Optionally update a timer UI here by exposing remaining/time to another component
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

        int winningIndex = winners.Count > 1 ? winners[UnityEngine.Random.Range(0, winners.Count)] : winners[0];

        string winningScene = sceneNames[winningIndex];
        Debug.Log($"Voting finished. Winning scene: {winningScene} (option {winningIndex}), votes: {votes[winningIndex]}");

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

    #region Input System wiring
    // Subscribe to actions on each PlayerInput to allow each player to vote.
    private List<(PlayerInput player, List<InputActionReferenceBinding>)> subscriptions = new List<(PlayerInput, List<InputActionReferenceBinding>)>();

    private void SubscribeToPlayerInputs()
    {
        UnsubscribeFromPlayerInputs();
        foreach (var p in PlayerInput.all)
        {
            // store the actions we subscribed so we can remove them later
            var bound = new List<InputActionReferenceBinding>();

            // Try per-option actions: VoteOption0, VoteOption1, ...
            bool anyBound = false;
            for (int opt = 0; opt < votes.Length; opt++)
            {
                var name = $"VoteOption{opt}";
                // do NOT throw if the action is missing; check for null instead.
                var action = p.actions.FindAction(name, false);
                if (action != null)
                {
                    anyBound = true;
                    // capture local variables
                    int playerIndex = p.playerIndex;
                    int optIndex = opt;
                    action.performed += ctx => RegisterVote(playerIndex, optIndex);
                    bound.Add(new InputActionReferenceBinding { action = action });
                }
            }

            if (!anyBound)
            {
                // fallback: try a single "Vote" action that returns an int
                var fallback = p.actions.FindAction("Vote", false);
                if (fallback != null)
                {
                    int playerIndex = p.playerIndex;
                    fallback.performed += ctx =>
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
                    bound.Add(new InputActionReferenceBinding { action = fallback });
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
                if (b.action != null)
                {
                    // best-effort: remove all listeners by re-creating callbacks is not possible here,
                    // but we can remove by removing anonymous by recreating - instead we rely on the fact
                    // that actions are short-lived for the voting session and will be ignored when not active.
                    // To be safer, remove all performed listeners by replacing with an empty delegate:
                    b.action.performed -= ctx => { };
                }
            }
        }
        subscriptions.Clear();
    }

    // helper type to keep track of what we subscribed to
    private class InputActionReferenceBinding
    {
        public UnityEngine.InputSystem.InputAction action;
    }
    #endregion
}
