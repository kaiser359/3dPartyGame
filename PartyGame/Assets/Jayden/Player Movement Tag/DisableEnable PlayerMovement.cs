using UnityEngine;

public class DisableEnablePlayerMovement : MonoBehaviour
{
    public static DisableEnablePlayerMovement instance;

    private Tag_Movement1[] gameObjects;
    public Timer gameObjectz;
   // public Tag_Movement1[] gameObjects;
<<<<<<< HEAD
   // public Timer[] gameObjectz;
=======
  //  public Timer[] gameObjectz;
>>>>>>> b437461845668ddbc44577d1fe969e415b8a28ff
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
        foreach (Tag_Movement1 player in gameObjects)
        {
            player.enabled = false;
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