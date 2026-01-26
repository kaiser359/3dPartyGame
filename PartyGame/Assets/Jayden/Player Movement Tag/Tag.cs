using UnityEngine;

public class Tag : MonoBehaviour
{
    [Tooltip("Color to apply while highlighted")]
    public Color highlightColor = Color.yellow;

    [Tooltip("If true, also respond to trigger events (OnTriggerEnter/Exit).")]
    public bool respondToTriggers = true;

    private SpriteRenderer spriteRenderer;
    private Renderer meshRenderer;
    private Color originalSpriteColor = Color.white;
    private Color originalMeshColor = Color.white;
    private bool hasSprite = false;
    private bool hasMesh = false;
    private bool isHighlighted = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            hasSprite = true;
            originalSpriteColor = spriteRenderer.color;
        }
        else
        {
            meshRenderer = GetComponent<Renderer>();
            if (meshRenderer != null)
            {
                hasMesh = true;
                // Use material.color to ensure we can change this instance's color
                originalMeshColor = meshRenderer.material.color;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Intentionally left blank - collisions/trigger events handle highlighting
    }

    private bool IsPlayerTag(GameObject obj)
    {
        // Accept both "player" and "Player" to match user text
        return obj != null && (obj.CompareTag("player") || obj.CompareTag("Player"));
    }

    private void SetHighlight(bool on)
    {
        if (isHighlighted == on) return;
        isHighlighted = on;

        if (hasSprite && spriteRenderer != null)
        {
            spriteRenderer.color = on ? highlightColor : originalSpriteColor;
            return;
        }

        if (hasMesh && meshRenderer != null)
        {
            // Accessing material creates an instance for this renderer which is fine here
            meshRenderer.material.color = on ? highlightColor : originalMeshColor;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsPlayerTag(collision.gameObject) && collision.gameObject != gameObject)
        {
            SetHighlight(true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (IsPlayerTag(collision.gameObject) && collision.gameObject != gameObject)
        {
            SetHighlight(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!respondToTriggers) return;
        if (IsPlayerTag(other.gameObject) && other.gameObject != gameObject)
        {
            SetHighlight(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!respondToTriggers) return;
        if (IsPlayerTag(other.gameObject) && other.gameObject != gameObject)
        {
            SetHighlight(false);
        }
    }
}
