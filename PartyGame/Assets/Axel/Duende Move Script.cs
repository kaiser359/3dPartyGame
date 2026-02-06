using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public enum State
    {
        normal, fleeing 
    }

    public Transform goal;
    public NavMeshAgent agent;
    public State currentstate;
    public float fleeDistance;
    public float stopDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
    private void Update()
    {
        if (currentstate == State.fleeing)
        {
            Transform player = GameObject.FindWithTag ("Player").transform;
            Vector3 newGoal = (player.position - transform.position).normalized * fleeDistance;

            agent.destination = newGoal;
        }
        else
        {
            agent.destination = goal.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(agent.destination, 1);
    }
}