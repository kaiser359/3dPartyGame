using UnityEngine;
using UnityEngine.AI;

public class musicial_chair : MonoBehaviour
{

    public bool taken = false;
    public GameObject player_model;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (taken && player_model != null)
        {
            player_model.transform.localEulerAngles = transform.parent.transform.localEulerAngles + new Vector3(0, 0, 0);
            print("totally rotating");
        }
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
                player_model = other.gameObject.GetComponent<Bot_movement>().player_model;
                other.gameObject.GetComponent<Bot_movement>().chair = this.gameObject.transform.parent.gameObject;
                player_model.transform.forward = -transform.up;
            }
        }
    }
}
