using UnityEngine;

public class Reflective : MonoBehaviour
{
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {

        Vector3 inDirection = rb.linearVelocity;

        // collision.contacts[0] gives the first point of contact
        Vector3 inNormal = collision.contacts[0].normal;

        Vector3 newVelocity = Vector3.Reflect(inDirection, inNormal);

        rb.linearVelocity = newVelocity;
    }
}
