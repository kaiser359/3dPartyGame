using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public string scoreKey = "Score";
    public int current = 0;

    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject Player;

    private bool warnedMissingText = false;

    void Start()
    {
        var loose = GetComponentInParent<LooseArrow>();
        

        if (scoreText == null && Player != null)
        {
            // Try to find a Text component on the spawner GameObject or its children
            scoreText = Player.GetComponent<Text>() ?? Player.GetComponentInChildren<Text>();
            if (scoreText == null)
            {
                warnedMissingText = false; // ensure one-time warning in Update
            }
        }
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {current}";
        }
        else
        {
            if (!warnedMissingText)
            {
                Debug.LogWarning("scoreText is not assigned and no Text component was found on the LooseArrow.spawner.");
                warnedMissingText = true;
            }
        }
    }
}
