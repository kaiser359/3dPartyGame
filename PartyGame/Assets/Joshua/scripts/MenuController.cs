using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
   
         public void Toggle(InputAction.CallbackContext context)
    {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

        }
    }
