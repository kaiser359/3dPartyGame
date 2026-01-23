using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenu : MonoBehaviour
{
    private bool _isPaused;

    [SerializeField] private GameObject _pauseMenuUI;

    public void Paused ()
    {
        _isPaused = true;
        _pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Resume ()
    {
        _isPaused = false;
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        _isPaused = !_isPaused;
        _pauseMenuUI.SetActive(_isPaused);

        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }



    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

}
