using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_stop_pushing_with_errors_bro : MonoBehaviour
{
    public void PlayGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
