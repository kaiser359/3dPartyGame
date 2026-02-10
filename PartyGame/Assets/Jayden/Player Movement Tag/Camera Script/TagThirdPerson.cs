using UnityEngine;
using UnityEngine.InputSystem;

public class tagthirdpersoncamera1 : MonoBehaviour
{
    public float sensitivity;

    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private float yRotation = 0f;

    public GameObject pivotx;
    public GameObject pivoty;
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        xRotation -= mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -20f, 89f);

        pivotx.transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);
    }

    public void Turn(InputAction.CallbackContext ctx)
    {
        mouseX = ctx.ReadValue<Vector2>().x * sensitivity;
        mouseY = ctx.ReadValue<Vector2>().y * sensitivity;
    }
}
