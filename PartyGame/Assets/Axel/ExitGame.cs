using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public void ChangeToVoteScene()
    {
        Debug.Log("Goodbye");
        Application.Quit();
    }

}
