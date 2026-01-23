using System.Collections;
using TMPro;
using UnityEngine;

public class mc_ui : MonoBehaviour
{
    public TMP_Text seat_text;
    public TMP_Text duel_info;
    public TMP_Text how2shoot;
    public TMP_Text shoot;
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
        yield return new WaitForSeconds(3f);
        duel_info.GetComponent<Animator>().SetTrigger("fade");
        yield return new WaitForSeconds(3.5f);
    }
}
