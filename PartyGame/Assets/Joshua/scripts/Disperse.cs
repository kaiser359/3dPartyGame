using UnityEngine;
using UnityEngine.InputSystem;

public class Disperse : MonoBehaviour
{
    // Which spawn slot this player should use (0..3)
    [Tooltip("Player index (0..3) selecting one of four spawn positions")]
    public int playerIndex = 0;

    [Tooltip("Optional: assign 4 spawn transforms (order: 0..3)")]
    public Transform[] spawnPoints;

    [Tooltip("Optional: prefab to instantiate. If null, this GameObject will be moved to the spawn.")]
    public GameObject playerPrefab;

    [Tooltip("If true, spawn (or move) on Start")]
    public bool spawnOnStart = true;

    public void SpawnAtIndex(PlayerInput player)
    {
        Debug.Log($"Disperse: Spawning player {player.playerIndex} at spawn point {player.playerIndex}");
        Vector3 pos = spawnPoints[player.playerIndex].position;
        Quaternion rot = spawnPoints[player.playerIndex].rotation;

        GameObject result = player.gameObject;
        result.name = $"Player_{player.playerIndex}";
        result.transform.SetPositionAndRotation(pos, rot);

    }
}
