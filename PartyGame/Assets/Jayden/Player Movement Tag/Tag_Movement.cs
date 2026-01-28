using System.Collections;
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

    // Player direction
    private Vector3 move;
    private float sideInput;
    private float forwardInput;

    [Header("Tagger Materials Settings")]
    //Materials
    public Material taggerMaterial;
    public Material taggerMaterial2;

    [Header("Player Materials Settings")]
    public Material runnerMaterial;

    // Dash state
    private bool canDash = true;
    private bool isDashing = false;

    private void Start()
    {
        // Ensure the correct material is applied at start based on role
        Material();
    }

    private void FixedUpdate()
    {
        float speedMultiplier = isTagger ? taggerSpeedMultiplier : 1f;

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

        if (isTagger)
        {
            // If both tagger materials are assigned choose one randomly for variety.
            if (taggerMaterial != null && taggerMaterial2 != null)
            {
                renderer.material = (UnityEngine.Random.value > 0.5f) ? taggerMaterial2 : taggerMaterial;
            }
            else if (taggerMaterial2 != null)
            {
                renderer.material = taggerMaterial2;
            }
            else
            {
                renderer.material = taggerMaterial;
            }
        }
        else
        {
            // Runner material for non-taggers
            if (runnerMaterial != null)
                renderer.material = runnerMaterial;
        }
    }
}


