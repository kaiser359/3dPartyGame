using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    public Image[] tabImages;
    public GameObject[] pages;

    // Hex strings (include leading '#')
    private readonly string inactiveHex = "#5204B9";
    private readonly string activeHex = "#621BBF";

    void Start()
    {
        ActivateTab(0);
    }

    public void ActivateTab(int tabNO)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            Color col;
            ColorUtility.TryParseHtmlString(inactiveHex, out col);
            tabImages[i].color = col;
        }

        pages[tabNO].SetActive(true);
        Color activeCol;
        ColorUtility.TryParseHtmlString(activeHex, out activeCol);
        tabImages[tabNO].color = activeCol;
    }
}