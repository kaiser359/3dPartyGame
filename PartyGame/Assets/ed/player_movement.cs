using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = System.Random;

public class player_movement : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody rb;
    public Transform tf;

    [Header("Player Settings")]
    public float acceleration;
    public float topSpeed;

    // Player direction
    private Vector3 move;
    private float sideInput;
    private float forwardInput;

    public bool in_duel = false;
    public bool can_shoot = false;
    public mc_ui ui;
    public musical_chairs_manager manager;
    public bool shoot_cooldown = false;

    public GameObject player_model;

    public GameObject pivoty;

    public Animator anim;
    private void FixedUpdate()
    {
        if (forwardInput == Mathf.Abs(1) && sideInput == Mathf.Abs(1))
        {
            Mathf.Sqrt(forwardInput);
            Mathf.Sqrt(sideInput);
        }
        move = tf.forward * forwardInput + tf.right * sideInput;
        move.y = 0;
        move.Normalize();
        Vector3 newVelocity = new Vector3(move.x * acceleration, 0, move.z * acceleration);
        rb.AddForce(newVelocity);
        print(rb.linearVelocity.sqrMagnitude);
        if (GetComponent<Bot_movement>().is_sitting == false && rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            if (GetComponent<Bot_movement>().alive)
            {
                player_model.transform.forward = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                anim.SetBool("moving", true);
            }
        }
        else
        {
            anim.SetBool("moving", true);
        }
    }

    public void give_shoot_tutorial()
    {
        ui.StartCoroutine(ui.shoot_tutorial());
    }
    public IEnumerator cooldown()
    {
        shoot_cooldown = true;
        yield return new WaitForSeconds(3f);
        shoot_cooldown = false;
    }

    public void shootnow()
    {
        ui.StartCoroutine(ui.shootnow());
    }

    public IEnumerator shoot_visual()
    {
        if (GetComponent<Bot_movement>().alive)
        {
            GetComponent<Bot_movement>().gun_flash.SetActive(true);
            GetComponent<Bot_movement>().gun.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.08f);
            GetComponent<Bot_movement>().gun_flash.SetActive(false);
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        sideInput = ctx.ReadValue<Vector2>().x;
        forwardInput = ctx.ReadValue<Vector2>().y;
    }
    public void Escape(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void Anykey(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        if (can_shoot && in_duel && !shoot_cooldown)
        {
            manager.StartCoroutine(manager.shoot_phase(GetComponent<Bot_movement>()));
            StartCoroutine(shoot_visual());
            in_duel = false;
            can_shoot = false;
        }
        if (!can_shoot && in_duel && !shoot_cooldown)
        {
            ui.StartCoroutine(ui.dont_shoot_yet());
            StartCoroutine(cooldown());
        }
    }
}

