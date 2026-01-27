using UnityEngine;
using UnityEngine.AI;

public class musicial_chair : MonoBehaviour
{

    public bool taken = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !taken || other.gameObject.CompareTag("bot") && !taken)
        {
            taken = true;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.position = transform.position + new Vector3(0, 1.85f, 0);
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            if (other.gameObject.GetComponent<NavMeshAgent>() != null)
            {
                other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            if (other.gameObject.GetComponent<Bot_movement>() != null)
            {
                other.gameObject.GetComponent<Bot_movement>().is_sitting = true;
            }
        }
    }
}
