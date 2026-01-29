using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelUpSystem : MonoBehaviour
{

    public static LevelUpSystem Instance;

    [Header("Abilities")]
   // public List<AbilityData> abilities = new List<AbilityData>();

    [HideInInspector] public List<int> abilityLevels = new List<int>();

    // New: maps for voting
    [Serializable]
    public class MapData
    {
        public string mapName;
        [TextArea] public string description;
        public Sprite icon;
        public string sceneName;
    }

    [Header("Maps")]
    public List<MapData> maps = new List<MapData>(); // add 5 maps in inspector

    [Header("UI")]
    public CanvasGroup levelUpCanvas;
    public Transform cardsParent;
    public GameObject cardPrefab;
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public int cardsToShow = 5; // show all maps by default
    public float votingDuration = 15f;
    public bool allowDuplicates = true;

    [Header("Game hooks (optional)")]
  //  public PlayerStats playerStats;

    private List<int> currentSelectionIndices = new List<int>();
    private List<GameObject> spawnedCards = new List<GameObject>();
    private System.Random rng = new System.Random();

    // voting state
    private int[] voteCounts;
    private Dictionary<int, int> playerVotes = new Dictionary<int, int>(); // playerIndex -> mapIndex
    private bool votingActive = false;
    private float votingRemaining = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        EnsureLevelsSize();

        if (levelUpCanvas != null)
        {
            levelUpCanvas.alpha = 0f;
            levelUpCanvas.blocksRaycasts = false;
            levelUpCanvas.interactable = false;
        }
    }

    private void EnsureLevelsSize()
    {
       // if (abilityLevels == null) abilityLevels = new List<int>();
     //   while (abilityLevels.Count < abilities.Count) abilityLevels.Add(0);
      //  if (abilityLevels.Count > abilities.Count)
       //     abilityLevels.RemoveRange(abilities.Count, abilityLevels.Count - abilities.Count);
    }

    // Public: start the voting UI (call when you want to present map choices)
    public void ShowVoting()
    {
        if (maps == null || maps.Count == 0)
        {
            Debug.LogWarning("LevelUpSystem: No maps defined.");
            return;
        }

        ClearSpawned();

        // choose which maps to show (if cardsToShow < maps.Count, pick random unique ones)
        currentSelectionIndices.Clear();

        var indices = Enumerable.Range(0, maps.Count).ToList();
        if (cardsToShow >= maps.Count)
        {
            currentSelectionIndices.AddRange(indices);
        }
        else
        {
            // pick unique indices
            for (int i = 0; i < cardsToShow; i++)
            {
                int pick = rng.Next(0, indices.Count);
                currentSelectionIndices.Add(indices[pick]);
                indices.RemoveAt(pick);
            }
        }

        // initialize vote counts
        voteCounts = new int[maps.Count];
        for (int i = 0; i < voteCounts.Length; i++) voteCounts[i] = 0;
        playerVotes.Clear();

        // spawn cards for selected maps
        spawnedCards.Clear();
        for (int i = 0; i < currentSelectionIndices.Count; i++)
        {
            int mapIndex = currentSelectionIndices[i];
            var map = maps[mapIndex];

            GameObject go = Instantiate(cardPrefab, cardsParent, false);
            spawnedCards.Add(go);

            LevelUpCard card = go.GetComponent<LevelUpCard>();
            if (card == null)
            {
                Debug.LogError("LevelUpSystem: cardPrefab missing LevelUpCard component.");
                continue;
            }

            string title = map.mapName;
            string desc = map.description ?? "";

            // Setup card and provide callback that registers an anonymous UI vote
            int capturedIndex = mapIndex;
            card.Setup(
                title,
                desc,
                map.icon,
                1,
                () => OnCardClicked(capturedIndex)
            );

            // ensure card shows votes
       //    card.SetVotes(0);
        }

        // select first card for keyboard/controller navigation if possible
        var firstSelectable = spawnedCards.FirstOrDefault();
        if (firstSelectable != null)
        {
            var es = transform.parent.GetComponentInChildren<EventSystem>();
            if (es != null)
            {
                var firstButton = firstSelectable.transform.GetChild(0).gameObject;
                es.SetSelectedGameObject(firstButton);
            }
            else if (es == null)
            {
                var firstButton = firstSelectable.transform.GetChild(0).gameObject;
                es.SetSelectedGameObject(firstButton);
            }
        }
       

        // show UI and start timer
        if (levelUpCanvas != null)
        {
            levelUpCanvas.alpha = 1f;
            levelUpCanvas.blocksRaycasts = true;
            levelUpCanvas.interactable = true;
        }

        votingActive = true;
        votingRemaining = votingDuration;
        UpdateTimerText();
    }

    private void OnCardClicked(int mapIndex)
    {
        // anonymous UI click: treat as a simple vote increment (no player tracking)
        if (voteCounts == null) return;
        voteCounts[mapIndex]++;
        UpdateCardVotesUI();
    }

    // External API: call this from a player input script to register a player's vote
    // playerIndex should be unique per local player (e.g., PlayerInput.playerIndex)
    public void Vote(int playerIndex, int mapIndex)
    {
        if (!votingActive) return;
        if (mapIndex < 0 || mapIndex >= maps.Count) return;

        // If the player already voted, remove previous vote
        if (playerVotes.TryGetValue(playerIndex, out int prev))
        {
            if (prev >= 0 && prev < voteCounts.Length)
                voteCounts[prev] = Math.Max(0, voteCounts[prev] - 1);
        }

        playerVotes[playerIndex] = mapIndex;
        voteCounts[mapIndex] = voteCounts[mapIndex] + 1;

        UpdateCardVotesUI();
    }

    private void Update()
    {
        if (!votingActive) return;

        votingRemaining -= Time.deltaTime;
        if (votingRemaining < 0f) votingRemaining = 0f;
        UpdateTimerText();

        if (votingRemaining <= 0f)
        {
            FinishVoting();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;
        timerText.text = Mathf.CeilToInt(votingRemaining).ToString();
    }

    private void FinishVoting()
    {
        votingActive = false;

        // pick top-voted map (tie -> random among top)
        if (voteCounts == null || voteCounts.Length == 0)
        {
            CloseLevelUp();
            return;
        }

        int maxVotes = voteCounts.Max();
        List<int> topIndices = new List<int>();
        for (int i = 0; i < voteCounts.Length; i++)
        {
            if (voteCounts[i] == maxVotes) topIndices.Add(i);
        }

        int chosenIndex = topIndices[rng.Next(0, topIndices.Count)];
        // Ensure chosenIndex is in the currentSelectionIndices; otherwise if we used voteCounts across all maps, it's fine.
        var chosenMap = maps[chosenIndex];

        Debug.Log($"LevelUpSystem: Voting finished. chosen map: {chosenMap.mapName} (scene: {chosenMap.sceneName}) votes: {maxVotes}");

        // Optionally hide UI immediately
        CloseLevelUp();

        // load the chosen scene (ensure scenes are added to build settings)
        if (!string.IsNullOrEmpty(chosenMap.sceneName))
        {
            SceneManager.LoadScene(chosenMap.sceneName);
        }
    }

    private void UpdateCardVotesUI()
    {
        if (spawnedCards == null || spawnedCards.Count == 0) return;

        // For each spawned card, find which map it represents and set votes
        for (int i = 0; i < spawnedCards.Count; i++)
        {
            var go = spawnedCards[i];
            if (go == null) continue;
            LevelUpCard card = go.GetComponent<LevelUpCard>();
            if (card == null) continue;

            // try to infer map index from the card title by matching to maps
            var titleText = card.titleText != null ? card.titleText.text : null;
            int mapIndex = -1;
            if (!string.IsNullOrEmpty(titleText))
            {
                for (int m = 0; m < maps.Count; m++)
                {
                    if (maps[m].mapName == titleText)
                    {
                        mapIndex = m;
                        break;
                    }
                }
            }

            // fallback: if mapIndex is still -1 and spawnedCards line up with currentSelectionIndices, use that
            if (mapIndex == -1 && i < currentSelectionIndices.Count)
                mapIndex = currentSelectionIndices[i];

            int votes = (mapIndex >= 0 && mapIndex < voteCounts.Length) ? voteCounts[mapIndex] : 0;
            card.SetVotes(votes);
        }
    }

    private void ClearSpawned()
    {
        foreach (var go in spawnedCards)
            if (go != null) Destroy(go);
        spawnedCards.Clear();
        currentSelectionIndices.Clear();
    }

    public void CloseLevelUp()
    {
        ClearSpawned();
        if (levelUpCanvas != null)
        {
            levelUpCanvas.alpha = 0f;
            levelUpCanvas.blocksRaycasts = false;
            levelUpCanvas.interactable = false;
        }

        votingActive = false;
    }
}


// LevelUpSystem.Instance.OnPlayerLeveledUp(); use it whne player levels up
