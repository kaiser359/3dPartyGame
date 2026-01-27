using UnityEngine;
using Random = System.Random;
public class Mazecontroller : MonoBehaviour
{
    // spawning
    public SpawnPoints[] sp;
    private Random rand = new Random();

    // players
    public PlayerMovement_MAZE[] pmM;

    private void Awake()
    {
        for (int i = 0; i < sp.Length; i++)
        {
            bool done = false;
            while (done == false)
            {
                int spPosition = rand.Next(0, sp.Length);
                if (sp[spPosition].taken == false)
                {
                    pmM[0].tf.position = sp[spPosition].tf.position;
                    sp[spPosition].taken = true;
                    done = true;
                }
            }
        }
    }
}
