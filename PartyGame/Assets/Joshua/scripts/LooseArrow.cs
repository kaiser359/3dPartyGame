using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing;

public class LooseArrow : MonoBehaviour
{
    public bool loose = false;
    public BowSway BowSway;
    public bool firstStep = false;

    // New flag to enable/disable shooting. Default true so shooting works unless disabled.
    public bool canShoot = true;

    public GameObject arrowPrefab;           
    public Transform arrowSpawnPoint;       
    public float arrowSpeed = 30f;
    public Score Player;
    public LooseArrow a;
    public WinStatement winStatement;

    public PlayerInput playerInput;

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    public void loosePrep(InputAction.CallbackContext context)
    {
        BowSway.Sway = BowSway.Direction.Vertical;
        canShoot = true;
    }

    public void Loose(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Prevent shooting if disabled
        if (!canShoot) return;

        BowSway.Sway = BowSway.Direction.Horizontal;
        canShoot = false;
        firstStep = true;
        loose = true;

        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrowInstance.GetComponent<arrow>().Player = Player;

       
        LooseArrow arrowComp = arrowInstance.GetComponent<LooseArrow>();
        if (arrowComp == null)
        {
            arrowComp = arrowInstance.AddComponent<LooseArrow>();
        }
        

        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
           
            rb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
        }
        else 
        {
            rb = arrowInstance.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        loose = false;
        firstStep = false;
    }
    public void ArcheryEnd()
    {
        
        BowSway.Sway = BowSway.Direction.None;
       
        Debug.Log("Archery Ended");

        
        Score[] allScores = FindObjectsByType<Score>(FindObjectsSortMode.None);
        if (allScores == null || allScores.Length == 0)
        {
            Debug.Log("No Score components found to tally.");
            return;
        }

        
        int highest = allScores.Where(s => s != null).Select(s => s.current).DefaultIfEmpty(int.MinValue).Max();

        
        List<Score> winners = allScores.Where(s => s != null && s.current == highest).ToList();

        if (winners.Count == 0)
        {
            Debug.Log("No valid scores found.");
            return;
        }

        if (winners.Count == 1)
        {
            Score winner = winners[0];
            string winnerName = winner.gameObject != null ? winner.gameObject.name : "Unknown";
            if (winnerName == "Player_0")
                winStatement.playerScore(1);
            else if (winnerName == "Player_1")
                winStatement.player2Score(1);
            else if (winnerName == "Player_2")
                winStatement.player3Score(1);
            else if (winnerName == "Player_3")
                winStatement.player4Score(1);
            Debug.Log($"Winner: {winnerName} with {highest} points");
        }
        else
        {
           
            string[] names = winners.Select(w => w.gameObject != null ? w.gameObject.name : "Unknown").ToArray();
            string namesJoined = string.Join(", ", names);
            Debug.Log($"Tie between players: {namesJoined} with {highest} points");
        }

        Destroy(a, 0.1f);
    }
}

