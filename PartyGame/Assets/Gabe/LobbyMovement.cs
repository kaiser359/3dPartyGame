using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyMovement : MonoBehaviour
{
    public SitOnchairTogetReady boo;
    private Vector2 _movement2 = Vector2.zero;
    public Vector3 lastMoveDirection;
    public float moveSpeed = 3f;

    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float gamepadSensitivity = 2f;
    [SerializeField] float mouseScale = 0.02f; 

    public Transform playerBody;      
    public Transform cameraTransform;

    private Vector2 lookInput;
    private float xRotation = 0f;
    private float currentSensitivity;
    private bool usingMouse;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        var pi = GetComponent<PlayerInput>();
        int index = (pi != null) ? pi.playerIndex : 0;
        if (boo == null)
            boo = Object.FindFirstObjectByType<SitOnchairTogetReady>();

        if (boo == null)
        {
            return;
        }

        while (boo.chairs.Count <= index)
            boo.chairs.Add(new ChairData());
        boo.chairs[index].player = this.gameObject;
    }

    void Start()
    {
        var pi = GetComponent<PlayerInput>();
        int i = (pi != null) ? pi.playerIndex : 0;

        // Ensure references: assume this script is on the player capsule.
        if (playerBody == null)
            playerBody = transform;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

      
    }

    void Update()
    {

        float scale = usingMouse ? mouseScale : Time.deltaTime;

        float yaw = lookInput.x * currentSensitivity * scale;
        float pitch = lookInput.y * currentSensitivity * scale;

        // Apply pitch to camera only so the capsule doesn't tilt.
        if (cameraTransform != null)
        {
            xRotation -= pitch; // subtract so moving mouse up looks up
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * yaw);
        if (_movement2 != Vector2.zero)
        {
            Vector3 localMove = new Vector3(_movement2.x, 0f, _movement2.y);
            Vector3 worldMove = playerBody.TransformDirection(localMove);
            transform.position += worldMove * (moveSpeed * Time.deltaTime);

            lastMoveDirection = worldMove.normalized;
        }
    }
    public void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && boo != null)
            boo.UnsetFirstReadyEntry();
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        _movement2 = input;
        if (input != Vector2.zero)
        {
            lastMoveDirection = new Vector3(input.x, 0f, input.y).normalized;
        }
    }public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

        var device = context.control?.device;
        if (device is Mouse)
        {
            currentSensitivity = mouseSensitivity;
            usingMouse = true;
        }
        else if (device is Gamepad)
        {
            currentSensitivity = gamepadSensitivity;
            usingMouse = false;
        }
        else
        {
            currentSensitivity = gamepadSensitivity;
            usingMouse = false;
        }
    }
}
