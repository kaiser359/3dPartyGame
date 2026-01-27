using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Random = System.Random;
public class Mazecontroller : MonoBehaviour
{
    // spawning
    public SpawnPoints[] sp;
    private Random rand = new Random();

    // players
    public List<PlayerMovement_MAZE> pmM;
    

    private void Awake()
    {
        // loop for placing players in map at random*
        for (int i = 0; i < pmM.Count; i++)
        {
            bool done = false;
            while (done == false)
            {
                int spPosition = rand.Next(0, sp.Length);
                Debug.Log(spPosition);
                if (sp[spPosition].taken == false)
                {
                    pmM[i].tf.position = sp[spPosition].tf.position;
                    sp[spPosition].taken = true;
                    done = true;
                }
            }
        }
        Debug.Log("test");
    }
    public void Join(PlayerInput player)
    {
        pmM.Add(player.GetComponent<PlayerMovement_MAZE>());
    }
}

