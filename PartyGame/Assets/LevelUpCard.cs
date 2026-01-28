using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelUpCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI countText;
    public Image iconImage;
    public Button button;
    private Action onClick;

    public void Setup(string title, string description, Sprite icon, int count, Action onClickCallback)
    {
        if (titleText != null) titleText.text = title;
        if (descriptionText != null) descriptionText.text = description;
        if (iconImage != null) iconImage.sprite = icon;
        if (countText != null) countText.text = count.ToString();
        onClick = onClickCallback;
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }

    // Add this method to support vote display
    public void SetVotes(int votes)
    {
        if (countText != null)
            countText.text = votes.ToString();
    }
}
