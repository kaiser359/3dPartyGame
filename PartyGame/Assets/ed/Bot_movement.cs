using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Bot_movement : MonoBehaviour
{
    public GameObject goal;
    public GameObject force_mover;
    public NavMeshAgent agent;
    public bool forcing = true;
    public bool is_sitting = false;
    public bool in_duel = false;
    public GameObject target_chair;
    public GameObject gun;
    public GameObject gun_flash;
    public musical_chairs_manager game_manager;
    public bool alive = true;

    public GameObject player_model;
    public GameObject chair;

    public Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (forcing == true)
            {
                agent.destination = goal.transform.position;
                player_model.transform.forward = agent.velocity.normalized;
            }
            else
            {
                if (in_duel == false)
                {
                    GameObject[] chairs = GameObject.FindGameObjectsWithTag("chair");
                    foreach (GameObject chair in chairs)
                    {
                        if (chair.GetComponent<musicial_chair>().taken == false)
                        {
                            target_chair = chair.gameObject;
                            agent.destination = target_chair.transform.position;
                            break;
                        }
                    }
                }
            }
            print(agent.velocity.normalized);
            if (!is_sitting || agent.velocity.normalized != new Vector3(0, 0, 0))
            {
                if (alive)
                {
                    player_model.transform.forward = agent.velocity.normalized;
                }
            }
            //Vector3 velocity = Vector3.ClampMagnitude(new(rb.linearVelocity.x, 0, rb.linearVelocity.z), 5);

            //Quaternion targetRotation = velocity == Vector3.zero ? transform.
            //rotation : Quaternion.LookRotation(velocity.normalized, Vector3.up);
            //if (velocity != Vector3.zero)
            //{
            //    player_model.transform.rotation = Quaternion.RotateTowards(player_model.transform.rotation, targetRotation, Time.deltaTime * 400);
            //}
        }
    }


    public IEnumerator shoot()
    {
        yield return new WaitForSeconds(Random.Range(0.4f, 0.8f));
        if (alive)
        {
            game_manager.StartCoroutine(game_manager.shoot_phase(this));
            gun_flash.SetActive(true);
            gun.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.08f);
            gun_flash.SetActive(false);
        }
    }

    public void Die()
    {
        alive = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        transform.position = transform.position + new Vector3(Random.Range(-3, 3), -1, (Random.Range(-3, 3)));
        player_model.transform.localEulerAngles = Vector3.zero;
        transform.localEulerAngles = transform.localEulerAngles + new Vector3(-90, 0, Random.Range(-180, 180));
        if (GetComponent<player_movement>() != null)
        {
            GetComponent<player_movement>().pivoty.transform.Rotate(-45, 0, 0);
        }
    }
}
