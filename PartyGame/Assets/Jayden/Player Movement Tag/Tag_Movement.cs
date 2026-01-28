using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tag_Movement : MonoBehaviour
{

    [Header("Player Components")]
    public Rigidbody rb;
    public Transform tf;

    [Header("Player Settings")]
    public float acceleration = 10f;
    public float topSpeed = 5f;

    [Header("Tagger Settings")]
    public bool isTagger = false;
    public float taggerSpeedMultiplier = 1.2f;

    [Header("Dash Settings")]
    public float dashForce = 10f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1.0f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    // Player direction
    private Vector3 move;
    private float sideInput;
    private float forwardInput;

    [Header("Materials Settings")]
    // Materials
    public Material taggerMaterial;
    public Material runnerMaterial;

    [Header("Outline Settings")]
    //public GameObject
    public GameObject obj;

    // Dash state
    private bool canDash = true;
    private bool isDashing = false;

    // Grounded state
    private bool isGrounded = false;


    private void Start()
    {

        // Ensure the correct material and outline visibility is applied at start based on role
        Material();
    }

    private void FixedUpdate()
    {
        float speedMultiplier = isTagger ? taggerSpeedMultiplier : 1f;

        // Update grounded state (safe-check groundCheck)
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
        }
        else
        {
            // fallback raycast if no groundCheck provided
            isGrounded = Physics.Raycast(tf.position, Vector3.down, 1.1f);
        }

        // Prevent diagonal inputs from exceeding magnitude 1
        if (move.sqrMagnitude > 1f)
            move = move.normalized;

        // While dashing, skip regular acceleration to preserve dash impulse feel
        if (!isDashing)
        {
            Vector3 accelerationForce = new(move.x * acceleration * speedMultiplier, 0f, move.z * acceleration * speedMultiplier);
            rb.AddForce(accelerationForce, ForceMode.Acceleration);
        }

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

    // Move action expects a Vector2 (e.g. from WASD or left stick)
    public void Move(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        print(v);
        sideInput = ctx.ReadValue<Vector2>().x;
        forwardInput = ctx.ReadValue<Vector2>().y;
    }

    // Jump action (bind a button to this, bind Space to the Jump action)
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Dash action (bind a button to this)
    public void Dash(InputAction.CallbackContext ctx)
    {
        // Only allow the current tagger to dash
        if (!isTagger)
            return;

        if (ctx.performed && canDash)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        isDashing = true;

        float speedMultiplier = isTagger ? taggerSpeedMultiplier : 1f;
        Vector3 dashDir = (move.sqrMagnitude > 0.01f) ? new Vector3(move.x, 0f, move.z).normalized : tf.forward;
        rb.AddForce(dashDir * dashForce * speedMultiplier, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // When this collider hits another player, transfer the tag if applicable
    private void OnCollisionEnter(Collision collision)
    {
        // Only the current tagger can tag others
        if (!isTagger)
            return;

        if (collision.gameObject.TryGetComponent<Tag_Movement>(out var other))
        {
            // If the other player is not the tagger, transfer the tag
            if (!other.isTagger)
            {
                TransferTag(other);
            }
        }
    }

    // Swap tag status: this player loses tag, other gains it
    private void TransferTag(Tag_Movement other)
    {
        other.isTagger = true;
        isTagger = false;

        // Update visuals immediately on both players
        other.Material();
        Material();

        Debug.Log($"{gameObject.name} tagged {other.gameObject.name}. Tag transferred.");
    }

    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Material()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
            return;

        // Use material at runtime to ensure instance when needed
        renderer.material = isTagger ? taggerMaterial : runnerMaterial;

        // Ensure `obj` is active only when this player is the tagger
        if (obj != null)
        {
            obj.SetActive(isTagger);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}