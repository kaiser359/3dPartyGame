using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class ManagerTag : MonoBehaviour
{
    // spawning
    public SpawnPoints[] sp;
    private Random rand = new Random();
    // players
    public List<Tag_Movement1> tmLi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < tmLi.Count; i++)
        {
            bool done = false;
            while (done == false)
            {
                int spPosition = rand.Next(0, sp.Length);
                if (sp[spPosition].taken == false)
                {
                    tmLi[i].tf.position = sp[spPosition].tf.position;
                    sp[spPosition].taken = true;
                    done = true;
                }
            }
        }
    }

    public void Join(PlayerInput player)
    {
        tmLi.Add(player.GetComponent<Tag_Movement1>());
    }
}
