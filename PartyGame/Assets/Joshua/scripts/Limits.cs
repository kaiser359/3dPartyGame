using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Limits : MonoBehaviour
{
    public float startTimeSeconds = 100f;
    public bool isRunning = true;
    public Text countdownText;
    public UnityEvent onTimerEnd;

    [SerializeField] private float remainingTime;
    public LooseArrow looseArrow;

    void Start()
    {
        remainingTime = Mathf.Max(0f, startTimeSeconds);

        
        if (countdownText == null)
            countdownText = GetComponent<Text>();

        if (countdownText == null)
        {
            Debug.LogError($"No text found on '{gameObject.name}'. Limits requires a Text component. Timer will not run.");
            isRunning = false;
            
            return;
        }

        UpdateText();

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isRunning = false;
            TimerEnded();
        }
    }

    void Update()
    {
        if (!isRunning)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            isRunning = false;
            TimerEnded();
        }

        UpdateText();
    }

    private void UpdateText()
    {
        if (countdownText == null)
            return;

        int seconds = Mathf.CeilToInt(Mathf.Max(0f, remainingTime));
        countdownText.text = seconds.ToString();
    }

    private void TimerEnded()
    {
        Debug.Log($"{gameObject.name} timer ended.");
        looseArrow.ArcheryEnd();
        onTimerEnd?.Invoke();
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public int GetRemainingSecondsRounded()
    {
        return Mathf.CeilToInt(remainingTime);
    }

    private void OnValidate()
    {
        if (startTimeSeconds < 0f)
            startTimeSeconds = 0f;
    }
}
