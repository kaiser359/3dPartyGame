using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class musical_chairs_manager : MonoBehaviour
{

    public GameObject force_mover;
    public GameObject bot;
    public int bot_count = 7;
    public mc_ui ui;
    public radio radio;
    public musical_chairs_light lightbulb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < bot_count; i++)
        {
            GameObject new_force_mover = Instantiate(force_mover);
            GameObject mover = new_force_mover.GetComponent<forcemove>().mover;
            new_force_mover.transform.Rotate(0, Random.Range(0, 360), 0);
            GameObject new_bot = Instantiate(bot, mover.transform.position, Quaternion.identity);
            new_bot.transform.SetParent(null);
            new_bot.GetComponent<Bot_movement>().goal = mover.transform;
        }
        StartCoroutine(tutorial());
    }

    public IEnumerator tutorial()
    {
        yield return StartCoroutine(ui.tutorial());
        lightbulb.lightson();
        yield return new WaitForSeconds(1f);
        radio.playsong();
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
