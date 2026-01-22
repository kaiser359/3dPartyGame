using UnityEngine;

public class BowSway : MonoBehaviour
{
    [SerializeField] private float swayAngle = 2f;
    [SerializeField] private float swaySpeed = 1.5f;         
    [SerializeField] private float smoothSpeed = 8f;         
    [SerializeField] private float inputThreshold = 0.02f;    
    [SerializeField] [Range(0f, 2f)] private float inputBiasStrength = 0.6f;

    private Quaternion originalLocalRotation;

    void Start()
    {
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float horiz = Input.GetAxis("Horizontal");
        float inputMagnitude = Mathf.Abs(mouseX) + Mathf.Abs(horiz);

       
        float idleSway = Mathf.Sin(Time.time * swaySpeed) * swayAngle;

        // Bias caused by player horizontal input (nudges the sway toward movement direction)
        float inputBias = 0f;
        if (inputMagnitude > inputThreshold)
        {
            // Prefer mouseX direction if present, otherwise use horizontal axis
            float dir = Mathf.Abs(mouseX) > Mathf.Epsilon ? Mathf.Sign(mouseX) : Mathf.Sign(horiz);
            inputBias = dir * swayAngle * inputBiasStrength;
        }

        // Combine idle oscillation with input bias to produce left-right sway value (yaw)
        float swayValue = idleSway + inputBias;

        // Target local rotation rotated around Y axis (left-right)
        Quaternion targetRotation = originalLocalRotation * Quaternion.Euler(0f, swayValue, 0f);

        // Smoothly interpolate local rotation toward the target
        float t = Mathf.Clamp01(smoothSpeed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, t);
    }
}

