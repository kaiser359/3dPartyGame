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
        // loop for filling out the array
        for(int i = 0; i < pmM.Length; i++)
        {
            pmM[i] = GetComponent<PlayerMovement_MAZE>();
        }
        // loop for placing players in map at random*
        for (int i = 0; i < pmM.Length; i++)
        {
            bool done = false;
            while (done == false)
            {
                int spPosition = rand.Next(0, sp.Length);
                if (sp[spPosition].taken == false)
                {
                    pmM[i].tf.position = sp[spPosition].tf.position;
                    sp[spPosition].taken = true;
                    done = true;
                }
            }
        }
    }
}
