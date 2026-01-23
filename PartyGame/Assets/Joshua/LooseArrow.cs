using UnityEngine;
using UnityEngine.InputSystem;

public class LooseArrow : MonoBehaviour
{

    public bool loose = false;
    public BowSway BowSway;

    public bool firstStep = false;


    public void loosePrep(InputAction.CallbackContext context)
    {

        if (!context.performed) return;


        if (!firstStep) return;

        BowSway.Sway = BowSway.Direction.Vertical;
    }


    public void Loose(InputAction.CallbackContext context)
    {

        if (!context.performed) return;

        BowSway.Sway = BowSway.Direction.None;


        firstStep = true;
        loose = true;
    }
}

