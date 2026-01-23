using UnityEngine;
using UnityEngine.InputSystem; // If using the new Input System

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Store player specific data, e.g., device references
    public InputDevice player1Device;
    public InputDevice player2Device;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add methods here to store and retrieve player info after selection
    public void SetPlayerDevices(InputDevice p1, InputDevice p2)
    {
        player1Device = p1;
        player2Device = p2;
    }
}
