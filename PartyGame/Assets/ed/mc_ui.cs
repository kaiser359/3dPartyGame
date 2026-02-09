using System.Collections;
using TMPro;
using UnityEngine;

public class mc_ui : MonoBehaviour
{
    public TMP_Text seat_text;
    public TMP_Text duel_info;
    public TMP_Text how2shoot;
    public TMP_Text shoot;
    public TMP_Text noshoot;
    public bool gun_cooldown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator tutorial()
    {
        seat_text.GetComponent<Animator>().SetTrigger("fade");
        yield return new WaitForSeconds(5f);
        duel_info.GetComponent<Animator>().SetTrigger("fade");
        yield return new WaitForSeconds(5.5f);
    }

    public IEnumerator shoot_tutorial()
    {
        how2shoot.GetComponent<Animator>().speed = 0.5f;
        how2shoot.GetComponent<Animator>().SetTrigger("fade");
        yield return new WaitForSeconds(5.5f);
    }

    public IEnumerator shootnow()
    {
        shoot.GetComponent<Animator>().speed = 3f;
        shoot.GetComponent<Animator>().SetTrigger("fade");
        yield return new WaitForSeconds(5.5f);
    }

    public IEnumerator dont_shoot_yet()
    {
        if (!gun_cooldown)
        {
            gun_cooldown = true;
            noshoot.GetComponent<Animator>().speed = 1.6666f;
            noshoot.GetComponent<Animator>().SetTrigger("fade");
            yield return new WaitForSeconds(3f);
            gun_cooldown = false;
        }
    }
}
