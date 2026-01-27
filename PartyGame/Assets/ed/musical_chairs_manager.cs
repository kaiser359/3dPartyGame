using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Linq;
public class musical_chairs_manager : MonoBehaviour
{

    public GameObject force_mover;
    public GameObject bot;
    public int bot_count = 7;
    public mc_ui ui;
    public radio radio;
    public musical_chairs_light lightbulb;
    public GameObject player;

    public GameObject duel_spot1;
    public GameObject duel_spot2;

    public Bot_movement dueler1;
    public Bot_movement dueler2;

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
            new_bot.GetComponent<Bot_movement>().goal = mover;
            agents.Add(new_bot.GetComponent<Bot_movement>());
        }
        GameObject new_player_force_mover = Instantiate(force_mover);
        GameObject player_mover = new_player_force_mover.GetComponent<forcemove>().mover;
        new_player_force_mover.transform.Rotate(0, Random.Range(0, 360), 0);
        player.GetComponent<Bot_movement>().goal = player_mover;
        player.transform.position = player_mover.transform.position;
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<Bot_movement>().enabled = false;
            player.GetComponent<player_movement>().enabled = true;
        }

        yield return new WaitUntil(() => FindObjectsByType<musicial_chair>(FindObjectsSortMode.None).All(item => item.taken));

        foreach (Bot_movement player in agents)
        {
            if (player.is_sitting == false)
            {
                if (dueler1 == null)
                {
                    dueler1 = player;
                    player.in_duel = true;
                    player.forcing = true;
                }
                else if (dueler2 == null)
                {
                    dueler2 = player;
                    player.in_duel = true;
                    player.forcing = true;
                }
            }
        }
        if (dueler1.GetComponent<player_movement>() != null)
        {
            dueler1.GetComponent<player_movement>().enabled = false;
        }
        if (dueler2.GetComponent<player_movement>() != null)
        {
            dueler2.GetComponent<player_movement>().enabled = false;
        }
        yield return new WaitForSeconds(1f);
        dueler1.goal = duel_spot1;
        dueler2.goal = duel_spot2;
        dueler1.gameObject.transform.position = duel_spot1.transform.position;
        dueler2.gameObject.transform.position = duel_spot2.transform.position;

    }
    void Update()
    {
        
    }
}
