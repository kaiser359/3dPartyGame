using UnityEngine;
using TMPro;

public class RunnerTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float remainingTime;

    public Tag_Movement[] gameObjects;
    public Chance[] chances;

    void Update()
    {
        if (!GetComponentInParent<Tag_Movement>().isTagger)
        {
            remainingTime += Time.deltaTime;
        }



        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
