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
    public enum Placement
    {
        First,
        Second,
        Third,
        Fourth
    }
    private Placement place;

    private void Start()
    {
        place = Placement.First;
        // loop for placing players in map at random*
        for (int i = 0; i < pmM.Count; i++)
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
        // undoing all the taken positions for respawning
        for(int i = 0; i < sp.Length; i++)
        {
            sp[i].taken = false;
        }
    }
    public void Join(PlayerInput player)
    {
        pmM.Add(player.GetComponent<PlayerMovement_MAZE>());
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement_MAZE pmM = collision.GetComponent<PlayerMovement_MAZE>();
            if (!pmM.done)
            {
                if(place == Placement.First)
                {
                    pmM.scoreHelp(3);
                    place = Placement.Second;
                }
                else if(place == Placement.Second)
                {
                    pmM.scoreHelp(2);
                    place = Placement.Third;
                }
                else if(place == Placement.Third)
                {
                    pmM.scoreHelp(1);
                    place = Placement.Fourth;
                }
                pmM.tf.position = new Vector3(0, 25, 0);
                pmM.done = true;
            }
        }
    }
}

