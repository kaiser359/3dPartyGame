using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public Slider play1;
    public Slider play2;
    public Slider play3;
    public Slider play4;

    public  PartyGameScore partyGameScore;

    private void Update()
    {
        play1.value = Mathf.Clamp(partyGameScore.Player1score / 20f, 0, 1);
        play2.value = partyGameScore.Player2score /20f;
        play3.value = partyGameScore.Player3score /20f;
        play4.value = partyGameScore.Player4score /20f ;
    }
}
