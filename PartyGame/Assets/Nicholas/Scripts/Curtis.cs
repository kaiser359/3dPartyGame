using UnityEngine;
using UnityEngine.AI;

public class Curtis : MonoBehaviour
{
    public NavMeshAgent nma;
    public Transform target;
    [SerializeField] BoxCollider bc;

    private void Update()
    {
        Target(target);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement_MAZE pmM = collision.GetComponent<PlayerMovement_MAZE>();
            pmM.die();
        }
    }
    private void Target(Transform t)
    {
        nma.SetDestination(t.position);
    }
}
