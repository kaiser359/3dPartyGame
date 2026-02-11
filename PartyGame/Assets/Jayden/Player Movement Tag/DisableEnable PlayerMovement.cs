using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DisableEnablePlayerMovement : MonoBehaviour
{
    public static DisableEnablePlayerMovement instance;
    public WinStatement winStatement;
    //private Tag_Movement1[] gameObjects;
  //  public Timer gameObjectz;
   
    void Start()
    {

    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {



    }

    public void DisablePlayerMovement()
    {
      
        gameObjects = FindObjectsByType<Tag_Movement1>(FindObjectsSortMode.None);
        if (gameObjects == null || gameObjects.Length == 0)
            return;

        foreach (Tag_Movement1 player in gameObjects)
        {
            player.enabled = false;
        }

    

        // Find the current tagger
        var tagger = gameObjects.FirstOrDefault(t => t != null && t.isTagger);

        if (winStatement != null && tagger != null)
        {
            // Give -2 to tagger, +1 to every other player
            int taggerIndex = GetPlayerIndexFromGameObject(tagger.gameObject);

            if (taggerIndex >= 0)
            {
                ApplyScoreChange(taggerIndex, -2);
            }

            foreach (var p in gameObjects)
            {
                if (p == null || p == tagger) continue;
                int idx = GetPlayerIndexFromGameObject(p.gameObject);
                if (idx >= 0)
                {
                    ApplyScoreChange(idx, +1);
                }
            }
        }
        else
        {
            Debug.LogWarning("DisableEnablePlayerMovement: WinStatement or tagger not found; score changes skipped.");
        }

        // Change scene to SampleScene
        SceneManager.LoadScene("SampleScene");
    }

   
    private int GetPlayerIndexFromGameObject(GameObject go)
    {
        if (go == null) return -1;

        var pi = go.GetComponent<PlayerInput>();
        if (pi != null) return pi.playerIndex;

       
        if (go.name != null)
        {
            var parts = go.name.Split('_');
            if (parts.Length >= 2 && int.TryParse(parts.Last(), out int parsed))
                return parsed;
        }

        return -1;
    }

   
    private void ApplyScoreChange(int playerIndex, int delta)
    {
        switch (playerIndex)
        {
            case 0:
                winStatement.playerScore(delta);
                break;
            case 1:
                winStatement.player2Score(delta);
                break;
            case 2:
                winStatement.player3Score(delta);
                break;
            case 3:
                winStatement.player4Score(delta);
                break;
            //default:
              //  break;
        }
    }

    public void EnablePlayerMovement()
    {
        gameObjects = FindObjectsByType<Tag_Movement1>(FindObjectsSortMode.None);
        foreach (Tag_Movement1 player in gameObjects)
        {
            player.enabled = true;
        }
    }

}