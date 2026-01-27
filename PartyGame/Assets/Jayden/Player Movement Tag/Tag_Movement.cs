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

    // Dash state
    private bool canDash = true;
    private bool isDashing = false;

    private void FixedUpdate()
    {
        float speedMultiplier = isTagger ? taggerSpeedMultiplier : 1f;

        // Build movement vector from inputs relative to transform
        move = tf.forward * forwardInput + tf.right * sideInput;

        // Prevent diagonal inputs from exceeding magnitude 1
        if (move.sqrMagnitude > 1f)
            move = move.normalized;

        // While dashing, skip regular acceleration to preserve dash impulse feel
        if (!isDashing)
        {
            Vector3 newVelocity = new Vector3(move.x * acceleration * speedMultiplier, 0f, move.z * acceleration * speedMultiplier);
            rb.AddForce(newVelocity, ForceMode.Acceleration);
        }

        // Clamp horizontal velocity
        Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        horizontal = Vector3.ClampMagnitude(horizontal, topSpeed * speedMultiplier);
        rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);
    }

    // Move action expects a Vector2 (e.g. from WASD or left stick)
    public void Move(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        sideInput = v.x;
        forwardInput = v.y;
    }

    // Dash action (bind a button to this)
    public void Dash(InputAction.CallbackContext ctx)
    {
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
        Debug.Log($"{gameObject.name} tagged {other.gameObject.name}. Tag transferred.");
    }

    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}


