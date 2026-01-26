using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Curtis : MonoBehaviour
{
    public NavMeshAgent nma;
    public PlayerMovement_MAZE[] targets;
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
                int player = rand.Next(0,targets.Length);
                if (targets[0].dead && targets[1].dead && targets[2].dead && targets[3].dead)
                {
                    Destroy(gameObject);
                    break;
                }
                else if (!targets[player].dead)
                {
                    Target(targets[player].tf);
                    done = true;
                    break;

                }
                
            }
        }
        if(elapsedTime >= 25)
        {
            elapsedTime = 0;
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
}
