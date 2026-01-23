using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LobbyMovement : MonoBehaviour
{

    //idk how it works but it does DONT MESS ON IT
    //i may not have a brain but i have ideas
    public SitOnchairTogetReady boo;

    // x = horizontal, y = vertical
    private Vector2 _movement2 = Vector2.zero;
    public Vector3 lastMoveDirection;
    public float moveSpeed = 3f;

    private void Awake()
    {
       
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        var pi = GetComponent<PlayerInput>();
        int i = (pi != null) ? pi.playerIndex : 0;
    }

    // Update is called once per frame - apply movement here
    void Update()
    {
        if (_movement2 != Vector2.zero)
        {
            
            Vector3 move = new Vector3(_movement2.x, 0f, _movement2.y);
            transform.Translate(move * (moveSpeed * Time.deltaTime), Space.World);

            lastMoveDirection = move.normalized;
        }
    }

    //  (PlayerInput using SendMessage or via action callback)
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
    }
}
