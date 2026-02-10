using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void ExitProgram()
    {
        Debug.Log("goodbye");
        Application.Quit();
    }
}
