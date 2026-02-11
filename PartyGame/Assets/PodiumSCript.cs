using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PodiumSCript : MonoBehaviour
{
    public PartyGameScore ww;
    public Transform number1;
    public Transform number2;
    public Transform number3;

    
    public GameObject player1, player2, player3, player4;

  
    public int requiredPoints = 20;

    // Ensures teleport happens only once
    private bool _teleported = false;

    void Update()
    {
        if (_teleported) return;

        if (ww == null)
        {
            Debug.LogWarning("PodiumSCript: PartyGameScore (ww) is not assigned.");
            return;
        }

       
        var players = new List<(GameObject player, int score, int index)>();

        foreach (var pi in PlayerInput.all)
        {
            if (pi == null || pi.gameObject == null) continue;
            int idx = pi.playerIndex;
            int score = GetScoreByIndex(idx);
            players.Add((pi.gameObject, score, idx));
        }


        AddFallbackPlayer(player1, 0, players);
        AddFallbackPlayer(player2, 1, players);
        AddFallbackPlayer(player3, 2, players);
        AddFallbackPlayer(player4, 3, players);

        if (players.Count == 0) return;

       
        if (!players.Any(p => p.score >= requiredPoints)) return;

    
        var ordered = players
            .OrderByDescending(p => p.score)
            .ThenBy(p => p.index)
            .ToList();

        // Teleport top 3 players (if present) to podium transforms
        if (ordered.Count > 0 && number1 != null)
            ordered[0].player.transform.position = number1.position;

        if (ordered.Count > 1 && number2 != null)
            ordered[1].player.transform.position = number2.position;

        if (ordered.Count > 2 && number3 != null)
            ordered[2].player.transform.position = number3.position;

      
        _teleported = true;
    }

   
    private int GetScoreByIndex(int index)
    {
        return index switch
        {
            0 => ww.Player1score,
            1 => ww.Player2score,
            2 => ww.Player3score,
            3 => ww.Player4score,
            _ => 0
        };
    }

    
    private void AddFallbackPlayer(GameObject fallback, int index, List<(GameObject player, int score, int index)> list)
    {
        if (fallback == null) return;
        if (list.Any(x => x.player == fallback)) return;
        int score = GetScoreByIndex(index);
        list.Add((fallback, score, index));
    }

    public void RunPodium()
    {
        _teleported = false;
        Update();
    }
}
