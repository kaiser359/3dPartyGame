using System;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

[System.Serializable]
public class ChairData
{
    public GameObject chair;
    public GameObject player;
    public bool ready;

    //[nonserialized]
     public Rigidbody playerRb;
     public LobbyMovement movementScript;
     public RigidbodyConstraints originalConstraints;
}

public class SitOnchairTogetReady : MonoBehaviour
{
    public List<ChairData> chairs = new List<ChairData>();
    public UnityEvent OnAllPlayersReady;
    public Action AllPlayersReady;
    public bool autoInvokeStartMinigame = true;
    private bool _minigameStarted = false;
    private void Start()
    {

        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null || entry.player == null) continue;
            entry.playerRb = entry.player.GetComponent<Rigidbody>();
            entry.movementScript = entry.player.GetComponent<LobbyMovement>();
            if (entry.playerRb != null)
                entry.originalConstraints = entry.playerRb.constraints;
        }
        if (chairs[0].movementScript == null)
        {
            chairs[0].movementScript = chairs[0].player.GetComponent<LobbyMovement>();

        }
        if (chairs[1].movementScript == null)
        {
            chairs[1].movementScript = chairs[1].player.GetComponent<LobbyMovement>();
        }
        else
        {
            Debug.Log("chair 1 movement script not null");
        }
        if (chairs[2].movementScript == null)
        {
            chairs[2].movementScript = chairs[2].player.GetComponent<LobbyMovement>();
        }
        else
        {
            Debug.Log("chair 2 movement script not null");

        }
        if (chairs[3].movementScript == null)
        {
            chairs[3].movementScript = chairs[2].player.GetComponent<LobbyMovement>();
        }
        else
        {
            Debug.Log("chair 2 movement script not null");
        }
    }
    private void Awake()
    {
  
    }
    private void OnTriggerEnter(Collider other)
    { 
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;
            if (other.gameObject == entry.player || other.gameObject == entry.chair)
            {
                entry.ready = true;
                //
                if (entry.player != null && entry.chair != null)
                {
                    entry.player.transform.position = entry.chair.transform.position;
                    entry.player.transform.rotation = entry.chair.transform.rotation;

                    if (entry.movementScript != null) entry.movementScript.enabled = false;
                    
                    if (entry.playerRb != null)
                    {
                        entry.playerRb.linearVelocity = Vector3.zero;
                        entry.playerRb.angularVelocity = Vector3.zero;
                        entry.playerRb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }   
                break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
     
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;
            if (other.gameObject == entry.player || other.gameObject == entry.chair)
            {
                UnlockEntry(entry);
                break;
            }
        }
    }

    private void Update()
    {
        // Don't re-check after we've already started the minigame
        

        int activePlayerCount = 0;
        int readyCount = 0;

        
        if (chairs[0].ready == true & chairs[0].movementScript != null)
            chairs[0].movementScript.enabled = false;
      
        if (chairs[1].ready == true & chairs[1].movementScript != null)
            chairs[1].movementScript.enabled = false;
        //third
       
        if (chairs[2].ready == true & chairs[2].movementScript != null)
            chairs[2].movementScript.enabled = false;
        else
        {
            Debug.Log("chair 2 movement script not null");
        }
      
        if (chairs[3].ready == true & chairs[2].movementScript != null)
            chairs[3].movementScript.enabled = false;
        if (_minigameStarted) return;
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;

            if (entry.player == null) continue;
            if (!entry.player.activeInHierarchy) continue;

            activePlayerCount++;
            if (entry.ready) readyCount++;
        }
        if (activePlayerCount > 0 && activePlayerCount == readyCount)
        {
            _minigameStarted = true;
          //  OnAllPlayersReady?.Invoke();    
            //AllPlayersReady?.Invoke();
            //placeholder
            if (autoInvokeStartMinigame)
                StartMinigame();
        }
    }
    public void UnsetFirstReadyEntry()
    {
        if (_minigameStarted) return;
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;
            if (entry.ready)
            {
                entry.ready = false;
                UnlockEntry(entry);

                if (entry.player != null && entry.chair != null)
                    entry.player.transform.position = entry.chair.transform.position + entry.chair.transform.forward * 1.0f;
                // guard against null movementScript to avoid NRE
                if (entry.movementScript != null)
                    entry.movementScript.enabled = true;

                break;
            }
        }
    }
    void UnlockEntry(ChairData entry)
    {

        if (entry == null) return;
        // Re-enable movement on unlock (was disabling previously)
        if (entry.movementScript != null) entry.movementScript.enabled = true;
        if (entry.playerRb != null)
        {
            entry.playerRb.constraints = entry.originalConstraints;
            entry.playerRb.linearVelocity = Vector3.zero;
            entry.playerRb.angularVelocity = Vector3.zero;
            //entry.movementScript = entry.player.GetComponent<LobbyMovement>();
        }
    }
    protected virtual void StartMinigame()
    {
        Debug.Log("All players are ready. StartMinigame() placeholder called. MWAHAHAHHHAHHAHAHAHAHAAAHHAHAHAHAHAHA");
        var cardsVote = FindAnyObjectByType<CardsVote>();
        if (cardsVote != null)
        {
            cardsVote.StartVoting();
        }
        else
        {
            Debug.LogError("CardsVote instance not found in the scene.");
        }
    }
}
