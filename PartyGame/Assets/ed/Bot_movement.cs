using UnityEngine;
using UnityEngine.AI;

public class Bot_movement : MonoBehaviour
{
    public GameObject goal;
    public NavMeshAgent agent;
    public bool forcing = true;
    public bool is_sitting = false;
    public bool in_duel = false;
    public GameObject target_chair;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (forcing == true)
        {
            agent.destination = goal.transform.position;
        }
        else
        {
            if (in_duel == false)
            {
                GameObject[] chairs = GameObject.FindGameObjectsWithTag("chair");
                foreach (GameObject chair in chairs)
                {
                    if (chair.GetComponent<musicial_chair>().taken == false)
                    {
                        target_chair = chair.gameObject;
                        agent.destination = target_chair.transform.position;
                        print(target_chair);
                        break;
                    }
                }
            }
        }
    }
}
