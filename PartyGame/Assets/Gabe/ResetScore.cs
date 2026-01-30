using UnityEngine;

public class ResetScore : MonoBehaviour
{
    public ScriptableObject scoradata;
    private void OnApplicationQuit()
    {
        scoradata.GetType().GetField("Player1score").SetValue(scoradata, 0);
        scoradata.GetType().GetField("Player2score").SetValue(scoradata, 0);
        scoradata.GetType().GetField("Player3score").SetValue(scoradata, 0);
        scoradata.GetType().GetField("Player4score").SetValue(scoradata, 0);
    }
}
