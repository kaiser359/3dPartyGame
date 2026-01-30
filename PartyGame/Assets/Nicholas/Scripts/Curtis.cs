using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Curtis : MonoBehaviour
{
    public NavMeshAgent nma;
    public List<PlayerMovement_MAZE> targets;
    public int target;
    private Random rand = new Random();
    public float elapsedTime;
    [SerializeField] BoxCollider bc;

    private void Start()
    {
        elapsedTime = 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime < 1)
        {
            bool done = false;
            while (done == false)
            {
                target = rand.Next(0,targets.Count);
                Target(targets[target].tf);
                done = true;
                break;
            }
        }
        if(elapsedTime >= 25 || targets[target].done)
        {
            elapsedTime = 0;
            FinishCheck();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement_MAZE pmM = collision.GetComponent<PlayerMovement_MAZE>();
            pmM.die();
            elapsedTime = 0f;
        }
    }
    private void Target(Transform t)
    {
        nma.SetDestination(t.position);
    }

    public void Join(PlayerInput player)
    {
        targets.Add(player.GetComponent<PlayerMovement_MAZE>());
    }

    private void FinishCheck()
    {
        int amountDone = 0;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].done)
            {
                amountDone++;
            }
        }
        if(amountDone == targets.Count)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("SampleScene");
        }
    }
}
