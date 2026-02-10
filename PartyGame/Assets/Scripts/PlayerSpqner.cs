using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpqner : MonoBehaviour
{
    public PlayerInputManager managerinput; 
    public GameObject playerPrefab;
    public GameObject playerPrefav2;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < InputSystem.devices.Count; ++i)
        {
            var device = InputSystem.devices[i];

            if (device.displayName == "Keyboard" || device.displayName == "Xbox Controller")
            {
                var input = managerinput.JoinPlayer(pairWithDevice: device);

                if (input.playerIndex == 0)
                {
                   // input.gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard");
                    //Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    managerinput.playerPrefab = playerPrefav2;
                }
               else if (input.playerIndex == 1)
                
                    {
                       // input.gameObject.GetComponent<PlayerInput>().SwitchCurrentControlScheme("Xbox Controller");
                    //    Instantiate(playerPrefav2, new Vector3(0, 0, 0), Quaternion.identity);
                    managerinput.playerPrefab = playerPrefab;
                }
                else if (input.playerIndex == 2)
                {
                    managerinput.playerPrefab = playerPrefav2;
                }
                else
                {
                    managerinput.playerPrefab = playerPrefab;
                }


            }
        }
    }
}
