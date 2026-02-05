using UnityEngine;
using UnityEngine.InputSystem;

public class tagthirdpersoncamera : MonoBehaviour
{
    public float sensitivity;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private float mouseX;
    private float mouseY;

    public GameObject pivotx;
    public GameObject pivoty;
    public GameObject Player;
    public void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); // makes it so you can look past legs and head

        pivotx.transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);
        Player.transform.localRotation = Quaternion.Euler(0f, -xRotation, 0f);
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        mouseX = ctx.ReadValue<Vector2>().x * sensitivity;
        mouseY = ctx.ReadValue<Vector2>().y * sensitivity;
    }
}
