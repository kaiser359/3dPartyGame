using UnityEngine;
using UnityEngine.InputSystem;

public class aSecondChance : MonoBehaviour
{
    public PlayerInputManager managerinput;
    public GameObject playerPrefab;
    public GameObject playerPrefav2;

    // base spawn position and spacing between spawned players
    public Vector3 spawnBase = Vector3.zero;
    public float spawnSpacing = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (managerinput == null)
        {
            Debug.LogError("PlayerSpqner: managerinput is not assigned.");
            return;
        }

        int spawnIndex = 0;

        for (int i = 0; i < InputSystem.devices.Count; ++i)
        {
            var device = InputSystem.devices[i];

            if (device.displayName == "Keyboard" || device.displayName == "Xbox Controller")
            {
                // choose prefab BEFORE joining so the created player uses the selected prefab
                // alternate prefabs (or implement your own selection logic here)
                managerinput.playerPrefab = (spawnIndex % 2 == 0) ? playerPrefav2 : playerPrefab;

                var input = managerinput.JoinPlayer(pairWithDevice: device);

                if (input != null)
                {
                    // place the newly spawned player spawnSpacing units apart along the X axis
                    input.gameObject.transform.position = spawnBase + Vector3.right * spawnIndex * spawnSpacing;

                    // optional: keep the old control-scheme switching comments for reference
                    if (input.playerIndex == 0)
                    {
                        // input.gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
                        //Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        // managerinput.playerPrefab = playerPrefav2; // handled above
                    }
                    else if (input.playerIndex == 1)
                    {
                        // input.gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Xbox Controller");
                        // Instantiate(playerPrefav2, new Vector3(0, 0, 0), Quaternion.identity);
                        // managerinput.playerPrefab = playerPrefab; // handled above
                    }
                }

                spawnIndex++;
            }
        }
    }
}
