using UnityEngine;

public class BowSway : MonoBehaviour
{
    public enum Direction
    {
        None,
        Horizontal,
        Vertical
    }

    [SerializeField] private float swayAngle = 2f;
    [SerializeField] private float swaySpeed = 1.5f;
    [SerializeField] private float smoothSpeed = 8f;
    [SerializeField] private float inputThreshold = 0.02f;
    [SerializeField][Range(0f, 2f)] private float inputBiasStrength = 0.6f;
    public Direction Sway;

    private Quaternion originalLocalRotation;
    [SerializeField] private float idleSwayYaw;
    [SerializeField] private float idleSwayPitch;

    void Start()
    {
        Sway = Direction.Horizontal;
        originalLocalRotation = transform.localRotation;

    }

    void Update()
    {
        // Apply horizontal sway only when enabled
        idleSwayYaw = Sway == Direction.Horizontal ? Mathf.Sin(Time.time * swaySpeed) * swayAngle : idleSwayYaw;

        // Apply vertical sway only when enabled
        idleSwayPitch = Sway == Direction.Vertical ? Mathf.Cos(Time.time * swaySpeed) * swayAngle : idleSwayPitch;

        Quaternion targetRotation = originalLocalRotation * Quaternion.Euler(idleSwayPitch, idleSwayYaw, 0f);

        float t = Mathf.Clamp01(smoothSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, t);


    }
}


