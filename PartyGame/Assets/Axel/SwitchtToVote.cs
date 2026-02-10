using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchtToVote : MonoBehaviour
{
    public void ChangeToVoteScene()
    {
        Debug.Log("hello");
        SceneManager.LoadScene("SampleScene");
    }
}
