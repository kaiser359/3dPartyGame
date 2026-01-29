using UnityEngine;
using UnityEngine.InputSystem;

public class LooseArrow : MonoBehaviour
{
    public bool loose = false;
    public BowSway BowSway;
    public bool firstStep = false;

    // New flag to enable/disable shooting. Default true so shooting works unless disabled.
    public bool canShoot = true;

    public GameObject arrowPrefab;           
    public Transform arrowSpawnPoint;       
    public float arrowSpeed = 30f;
    public Score Player;

    public void loosePrep(InputAction.CallbackContext context)
    {
        BowSway.Sway = BowSway.Direction.Vertical;
        canShoot = true;
    }

    public void Loose(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Prevent shooting if disabled
        if (!canShoot) return;

        BowSway.Sway = BowSway.Direction.Horizontal;
        canShoot = false;
        firstStep = true;
        loose = true;

        GameObject arrowInstance = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        arrowInstance.GetComponent<arrow>().Player = Player;

       
        LooseArrow arrowComp = arrowInstance.GetComponent<LooseArrow>();
        if (arrowComp == null)
        {
            arrowComp = arrowInstance.AddComponent<LooseArrow>();
        }
        

        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
           
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
    public void ArcheryEnd()
    {
        canShoot = false;
        BowSway.Sway = BowSway.Direction.None; 

    }
}

