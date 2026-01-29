using UnityEngine;

public class thirdpersoncamera : MonoBehaviour
{
    public float sensitivity;
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
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation -= mouseX;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -20f, 90f);

        pivotx.transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);
    }
}
