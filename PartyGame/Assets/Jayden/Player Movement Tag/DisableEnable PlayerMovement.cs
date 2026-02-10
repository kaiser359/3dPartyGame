using UnityEngine;

public class DisableEnablePlayerMovement : MonoBehaviour
{
    public static DisableEnablePlayerMovement instance;

    public Tag_Movement[] gameObjects;
    public Timer[] gameObjectz;
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
        gameObjects = FindObjectsByType<Tag_Movement>(FindObjectsSortMode.None);
        foreach (Tag_Movement player in gameObjects)
        {
            player.enabled = false;
        }
    }

    public void EnablePlayerMovement()
    {
        gameObjects = FindObjectsByType<Tag_Movement>(FindObjectsSortMode.None);
        foreach (Tag_Movement player in gameObjects)
        {
            player.enabled = true;
        }
    }

}