using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public string scoreKey = "Score";
    public int current = 0;

    [SerializeField] private Text scoreText;

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {current}";
        }
        else
        {
            Debug.LogWarning("scoreText is not assigned on TargetPoints.");
        }

    }
}
