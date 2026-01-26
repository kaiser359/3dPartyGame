using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpqner : MonoBehaviour
{
    public PlayerInputManager managerinput; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < InputSystem.devices.Count; ++i)
        {
            var device = InputSystem.devices[i];

            if (device.displayName == "Keyboard" || device.displayName == "Xbox Controller")
            {
                var input = managerinput.JoinPlayer(pairWithDevice: device);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
