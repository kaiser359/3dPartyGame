using UnityEngine;
using UnityEngine.InputSystem;

public class LooseArrow : MonoBehaviour
{
    // Public configurable fields
    public bool loose = false;
    public BowSway BowSway;
    public bool firstStep = false;
    public GameObject spawner;

    // Arrow spawn configuration
    public GameObject arrowPrefab;           
    public Transform arrowSpawnPoint;       
    public float arrowSpeed = 30f;
    public Score Player;

    public void loosePrep(InputAction.CallbackContext context)
    {
        BowSway.Sway = BowSway.Direction.Vertical;
    }

    public void Loose(InputAction.CallbackContext context)
    {
        // Only act on performed to avoid multiple triggers
        if (!context.performed) return;

        BowSway.Sway = BowSway.Direction.Horizontal;
        firstStep = true;
        loose = true;

        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrowInstance.GetComponent<arrow>().Player = Player;

        // Record who spawned this arrow
        LooseArrow arrowComp = arrowInstance.GetComponent<LooseArrow>();
        if (arrowComp == null)
        {
            arrowComp = arrowInstance.AddComponent<LooseArrow>();
        }
        arrowComp.spawner = gameObject;

        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Use Rigidbody.velocity (not linearVelocity)
            rb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
        }
        else
        {
            rb = arrowInstance.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            rb.linearVelocity = arrowSpawnPoint.forward * arrowSpeed;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        loose = false;
        firstStep = false;
    }
}

