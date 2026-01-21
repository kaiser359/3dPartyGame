using UnityEngine;
using UnityEngine.AI;

public class Bot_movement : MonoBehaviour
{
    public Transform goal;
    public NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.transform.position;
    }
}
