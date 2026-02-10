using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneToVote : MonoBehaviour
{
    
    public void ChangeToVoteScene()
    {
        Debug.Log("hello");
        SceneManager.LoadScene("SampleScene");
    }
}
