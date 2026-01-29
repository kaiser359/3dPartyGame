using UnityEngine;

public class HitBoxTag : MonoBehaviour
{
    [Tooltip("Range to check for targets")]
    public float radius = 5f;

    [Tooltip("Only consider objects with this tag")]
    public string targetTag = "Player";

    [Tooltip("If true, use Unity tags. Otherwise add a marker component.")]
    public bool useUnityTag = false;

    [Tooltip("If using Unity tags, assign the tag name to apply (must exist in Tag Manager)")]
    public string applyTag = "Tagged";

    [Tooltip("Layer mask to narrow checks (optional)")]
    public LayerMask layerMask = ~0;

    [Tooltip("How often to check (seconds) - reduce frequency for better performance)")]
    public float checkInterval = 0.2f;

    float nextCheck = 0f;

    void Update()
    {
        // Only perform the range check on left mouse button click (mouse button 0)
        // and respect the check interval to avoid over-checking.
        if (Input.GetMouseButtonDown(0) && Time.time >= nextCheck)
        {
            nextCheck = Time.time + checkInterval;
            CheckRange();
        }
    }

    void CheckRange()
    {
        // Only proceed if this object is currently the tagger.
        var ownerTM = GetComponent<Tag_Movement>();
        if (ownerTM == null || !ownerTM.isTagger) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (var col in hits)
        {
            GameObject go = col.gameObject;
            if (!go.CompareTag(targetTag)) continue;

            // Try to transfer tagger state if the hit object has Tag_Movement
            if (go.TryGetComponent<Tag_Movement>(out var targetTM))
            {
                if (!targetTM.isTagger)
                {
                    // Transfer the tagger role
                    targetTM.isTagger = true;
                    ownerTM.isTagger = false;

                    // Apply visual/marker if configured
                    if (useUnityTag)
                    {
                        go.tag = applyTag;
                    }
                    else
                    {
                        if (!go.TryGetComponent<RangeTagged>(out _))
                            go.AddComponent<RangeTagged>();
                    }

                    // Stop after successful transfer to avoid multiple transfers in one check
                    break;
                }
                else
                {
                    // Already a tagger; still optionally mark it
                    if (useUnityTag)
                    {
                        go.tag = applyTag;
                    }
                    else
                    {
                        if (!go.TryGetComponent<RangeTagged>(out _))
                            go.AddComponent<RangeTagged>();
                    }
                }
            }
            else
            {
                // No Tag_Movement on target: only apply the visual/marker if desired
                if (useUnityTag)
                {
                    go.tag = applyTag;
                }
                else
                {
                    if (!go.TryGetComponent<RangeTagged>(out _))
                        go.AddComponent<RangeTagged>();
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
