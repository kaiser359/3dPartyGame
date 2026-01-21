using UnityEngine;

public class WinStatement : MonoBehaviour
{
    public PartyGameScore partyPoints;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void playerScore(int score)
    {
        partyPoints.Player1score += score; 
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
