using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Random = System.Random;

public class PlayerMovement_MAZE : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb;
    public Transform tf;
    public Mazecontroller mc;
    public WinStatement ws;
    public PlayerInput pi;


    [Header("Player Settings")]
    public float acceleration;
    public float topSpeed;

    // Player direction
    private Vector3 move;
    private float sideInput;
    private float forwardInput;

    private Random rand = new Random();

    public bool done;
    private void Awake()
    {
        mc = FindAnyObjectByType<Mazecontroller>();
        ws = FindAnyObjectByType<WinStatement>();
    }
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

    public void Move(InputAction.CallbackContext ctx)
    {
        sideInput = ctx.ReadValue<Vector2>().x;
        forwardInput = ctx.ReadValue<Vector2>().y;
    }
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void score()
    {
        if(pi.playerIndex == 0)
        {

        }
    }
    public void die()
    {
        int spPosition = rand.Next(0, mc.sp.Length);
        tf.position = mc.sp[spPosition].tf.position;
    }
    public int getPlayerIndex()
    {
        return pi.playerIndex;
    }
    public void scoreHelp(int points)
    {
        if(pi.playerIndex == 0)
        {
            ws.playerScore(points);
        }
        else if (pi.playerIndex == 1)
        {
            ws.player2Score(points);
        }
        else if (pi.playerIndex == 2)
        {
            ws.player3Score(points);
        }
        else if (pi.playerIndex == 3)
        {
            ws.player4Score(points);
        }
    }
}
