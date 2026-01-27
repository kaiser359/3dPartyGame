using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
public class musical_chairs_manager : MonoBehaviour
{

    public GameObject force_mover;
    public GameObject bot;
    public int bot_count = 7;
    public mc_ui ui;
    public radio radio;
    public musical_chairs_light lightbulb;
    public GameObject player;

    public List<Bot_movement> agents = new List<Bot_movement>();
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
            agents.Add(new_bot.GetComponent<Bot_movement>());
        }
        GameObject new_player_force_mover = Instantiate(force_mover);
        GameObject player_mover = new_player_force_mover.GetComponent<forcemove>().mover;
        new_player_force_mover.transform.Rotate(0, Random.Range(0, 360), 0);
        player.GetComponent<Bot_movement>().goal = player_mover.transform;
        agents.Add(player.GetComponent<Bot_movement>());
        StartCoroutine(tutorial());
    }

    public IEnumerator tutorial()
    {
        yield return StartCoroutine(ui.tutorial());
        lightbulb.lightson();
        yield return new WaitForSeconds(1f);
        StartCoroutine(round());
    }

    public IEnumerator round()
    {
        radio.playsong();
        yield return new WaitForSeconds(Random.Range(4f, 15f));
        radio.stopsong();
        foreach (Bot_movement movement in agents)
        {
            movement.forcing = false;
        }
    }
    void Update()
    {
        
    }
}
