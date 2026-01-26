using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = System.Random;

public class PlayerMovement_MAZE : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb;
    public Transform tf;

    [Header("Player Settings")]
    public float acceleration;
    public float topSpeed;
    public bool dead;

    // Player direction
    private Vector3 move;
    private float sideInput;
    private float forwardInput;

    // spawning
    public SpawnPoints[] sp;
    private Random rand = new Random();

    private void FixedUpdate()
    {
        if (forwardInput == Mathf.Abs(1) && sideInput == Mathf.Abs(1))
        {
            Mathf.Sqrt(forwardInput);
            Mathf.Sqrt(sideInput);
        }
        move = tf.forward * forwardInput + tf.right * sideInput;
        Vector3 newVelocity = new Vector3(move.x * acceleration, 0, move.z * acceleration);
        rb.AddForce(newVelocity);
        Vector3 velocity = Vector3.ClampMagnitude(new(rb.linearVelocity.x, 0, rb.linearVelocity.z), topSpeed);
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }
    private void Awake()
    {
        bool done = false;
        while (done == false)
        {
            int spPosition = rand.Next(0, sp.Length);
            if (sp[spPosition].taken == false)
            {
                tf.position = sp[spPosition].tf.position;
                sp[spPosition].taken = true;
                done = true;
            }
        }
        
        
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        sideInput = ctx.ReadValue<Vector3>().x;
        forwardInput = ctx.ReadValue<Vector3>().z;
    }
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void die()
    {
        tf.position = new Vector3(0, 25, 0);
        dead = true;
    }
}
