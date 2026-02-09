using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public void QuickQuit(InputAction.CallbackContext context)
    {
        if (menuCanvas.activeSelf)
        {
            SceneManager.LoadScene("SampleScene");
           menuCanvas.SetActive(false);
           
        }
    }
}
