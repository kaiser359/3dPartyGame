using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LooseArrow : MonoBehaviour
{
    // When true, indicates the arrow has been loosed (released).
    public bool loose = false;
    public BowSway BowSway;
    public bool firstStep = false;

    public void loosePrep(InputAction.CallbackContext context)
    {
        
        BowSway.Sway = BowSway.Direction.Vertical;

        
    }
    public void Loose(InputAction.CallbackContext context)
    {
        BowSway.Sway = BowSway.Direction.None;
    }
}

   

