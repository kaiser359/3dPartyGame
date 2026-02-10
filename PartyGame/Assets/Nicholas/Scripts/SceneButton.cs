using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public string sceneName;

    public void OnTriggerEnter(Collider col)
    {
        if (sceneName == "Tag")
        {
            SceneManager.LoadScene("Tag_Map_1");
        }
        else if (sceneName == "Maze") {
            SceneManager.LoadScene("Maze");
        }
		else if (sceneName == "Music")
		{
			SceneManager.LoadScene("musical chairs");
		}
		else if (sceneName == "Archery")
		{
			SceneManager.LoadScene("Archery");
		}

	}
}
