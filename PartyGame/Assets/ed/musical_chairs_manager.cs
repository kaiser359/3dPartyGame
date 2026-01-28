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
    public GameObject duel_center;

    public Bot_movement dueler1;
    public Bot_movement dueler2;

    public bool shooting_phase = false;

    public List<Bot_movement> agents = new List<Bot_movement>();
    public List<GameObject> chairs = new List<GameObject>();
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
            new_bot.GetComponent<Bot_movement>().force_mover = mover;
            new_bot.GetComponent<Bot_movement>().game_manager = this;
            agents.Add(new_bot.GetComponent<Bot_movement>());
        }
        GameObject new_player_force_mover = Instantiate(force_mover);
        GameObject player_mover = new_player_force_mover.GetComponent<forcemove>().mover;
        new_player_force_mover.transform.Rotate(0, Random.Range(0, 360), 0);
        player.GetComponent<Bot_movement>().goal = player_mover;
        player.GetComponent<Bot_movement>().force_mover = player_mover;
        player.GetComponent<Bot_movement>().game_manager = this;
        player.transform.position = player_mover.transform.position;
        agents.Add(player.GetComponent<Bot_movement>());
        StartCoroutine(tutorial());

        GameObject[] chairs2 = GameObject.FindGameObjectsWithTag("chair");
        foreach (GameObject chair in chairs2)
        {
            chairs.Add(chair);
        }
    }

    public IEnumerator tutorial()
    {
        yield return StartCoroutine(ui.tutorial());
        lightbulb.lightson();
        duel_center.GetComponent<AudioSource>().Pause();
        yield return new WaitForSeconds(1f);
        StartCoroutine(round());
    }

    public IEnumerator round()
    {
        radio.playsong();
        yield return new WaitForSeconds(Random.Range(6f, 15f));
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
            if (player.is_sitting == false && player.alive == true)
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
        dueler1.gun.SetActive(true);
        dueler2.gun.SetActive(true);
        duel_spot1.transform.position = new Vector3(0, 0, 2);
        duel_spot2.transform.position = new Vector3(0, 0, -2);
        dueler1.gameObject.transform.position = duel_spot1.transform.position;
        dueler2.gameObject.transform.position = duel_spot2.transform.position;
        yield return new WaitForSeconds(2f);
        duel_center.GetComponent<AudioSource>().Play();
        float random_time = Random.Range(8f, 16f);
        float timeElapsed = 0;
        while (timeElapsed < random_time)
        {
            duel_spot1.transform.position += new Vector3(0, 0, Time.deltaTime/10);
            duel_spot2.transform.position += new Vector3(0, 0, -Time.deltaTime/10);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        lightbulb.lightsoff();
        duel_center.GetComponent<AudioSource>().Pause();
        dueler1.StartCoroutine(dueler1.shoot());
        dueler2.StartCoroutine(dueler2.shoot());
        shooting_phase = true;
        lightbulb.rotx += 2f;
        lightbulb.rotz += 2f;
        lightbulb.speedx += 0.2f;
        lightbulb.speedz += 0.4f;
        lightbulb.min_intense -= 3f;
        lightbulb.max_intense += 3f;
}

    public IEnumerator shoot_phase(Bot_movement suriving_dueler)
    {
        if (shooting_phase == true)
        {
            if (suriving_dueler == dueler1)
            {
                dueler2.Die();
            }
            if (suriving_dueler == dueler2)
            {
                dueler1.Die();
            }
            shooting_phase = false;
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            lightbulb.lightson();
            yield return new WaitForSeconds(3f);
            suriving_dueler.gun.SetActive(false);
            int randomchair = Random.Range(0, chairs.Count);
            GameObject chairremove = chairs[randomchair];
            chairs.RemoveAt(randomchair);
            Destroy(chairremove);
            dueler1 = null;
            dueler2 = null;
            foreach (GameObject chair in chairs)
            {
                chair.GetComponent<musicial_chair>().taken = false;
            }
            Debug.Log("Test");
            foreach (Bot_movement movement in agents)
            {
                if (movement.alive == true)
                {
                    movement.goal = movement.force_mover;
                    movement.gameObject.transform.position = movement.goal.transform.position;
                    movement.forcing = true;
                    movement.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    movement.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    movement.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                    movement.is_sitting = false;
                    movement.in_duel = false;
                }
            }
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<NavMeshAgent>().enabled = true;
                player.GetComponent<Bot_movement>().enabled = true;
                player.GetComponent<player_movement>().enabled = false;
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(round());
        }
    }
}
