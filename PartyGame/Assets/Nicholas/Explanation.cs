using UnityEngine;

public class Explanation : MonoBehaviour
{
    public WinStatement win;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        win.playerScore(3);
    }
}
