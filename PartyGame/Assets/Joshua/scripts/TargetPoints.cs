using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TargetPoints : MonoBehaviour
{
    public int pointsToAdd;
    [SerializeField] private bool destroyArrowOnHit = true;

    

    public string scoreKey = "Score";

  
    private void HandleHit(GameObject other)
    {
        if (other == null) return;

        // Debug to help see what was hit when score doesn't change
        Debug.Log($"Hit object tag: {other.tag}, name: {other.name}");

        if (other.CompareTag("arrow"))
        {
            GetComponentInParent<Score>().current += pointsToAdd;
            if (destroyArrowOnHit)
            {
                Destroy(other);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }
}
