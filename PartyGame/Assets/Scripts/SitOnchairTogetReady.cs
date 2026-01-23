using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[System.Serializable]
public class ChairData
{
    public GameObject chair;
    public GameObject player;
    public bool ready;

    // runtime cached fields (not serialized)
    [NonSerialized] public Rigidbody playerRb;
    [NonSerialized] public PlayerMovement_MAZE movementScript;
    [NonSerialized] public RigidbodyConstraints originalConstraints;
}

public class SitOnchairTogetReady : MonoBehaviour
{
    public List<ChairData> chairs = new List<ChairData>();

    // assign this to the "jump" action (space) from the Input System in the inspector
    public InputActionReference jumpAction;

    private void Start()
    {
        // cache references for each entry if possible
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null || entry.player == null) continue;
            entry.playerRb = entry.player.GetComponent<Rigidbody>();
            entry.movementScript = entry.player.GetComponent<PlayerMovement_MAZE>();
            if (entry.playerRb != null)
                entry.originalConstraints = entry.playerRb.constraints;
        }
    }

  

    private void OnTriggerEnter(Collider other)
    {
        // mark the matching chair/player pair as ready when the player enters the trigger
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;

            
            if (other.gameObject == entry.player || other.gameObject == entry.chair)
            {
                entry.ready = true;

                // snap player to chair position and lock movement
                if (entry.player != null && entry.chair != null)
                {
                    entry.player.transform.position = entry.chair.transform.position;
                    // disable movement script
                    if (entry.movementScript != null) entry.movementScript.enabled = false;
                    // stop physics motion and freeze rigidbody if present
                    if (entry.playerRb != null)
                    {
                        entry.playerRb.linearVelocity = Vector3.zero;
                        entry.playerRb.angularVelocity = Vector3.zero;
                        entry.playerRb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }

                Debug.Log("Player is ready: " + (entry.player ? entry.player.name : "unknown"));
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

    }



    public void UnsetFirstReadyEntry()
    {
        // un-sets "ready" for any currently-ready entry and unlocks the player
        for (int i = 0; i < chairs.Count; i++)
        {
            var entry = chairs[i];
            if (entry == null) continue;
            if (entry.ready)
            {
                entry.ready = false;
                UnlockEntry(entry);

                // nudge player forward a bit so they are no longer exactly at the chair origin
                if (entry.player != null && entry.chair != null)
                    entry.player.transform.position = entry.chair.transform.position + entry.chair.transform.forward * 1.0f;

                Debug.Log("Player is no longer ready: " + (entry.player ? entry.player.name : "unknown"));
                // break if only want to affect a single player per press
                break;
            }
        }
    }

    void UnlockEntry(ChairData entry)
    {
        if (entry == null) return;

        // restore movement script
        if (entry.movementScript != null) entry.movementScript.enabled = true;

        // restore rigidbody constraints
        if (entry.playerRb != null)
        {
            entry.playerRb.constraints = entry.originalConstraints;
            entry.playerRb.linearVelocity = Vector3.zero;
            entry.playerRb.angularVelocity = Vector3.zero;
        }
    }
}
