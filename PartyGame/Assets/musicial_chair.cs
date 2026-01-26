using UnityEngine;

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
        if (other.gameObject.CompareTag("Player") && !taken)
        {
            taken = true;
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.position = transform.position + new Vector3(0, 1.85f, 0);
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }
}
