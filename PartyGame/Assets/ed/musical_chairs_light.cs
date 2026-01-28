using UnityEngine;

public class musical_chairs_light : MonoBehaviour
{
    public Light lightbulb;
    public Light outer_light;
    public float rotx = 0f;
    public float rotz = 0f;
    public float speedx = 0f;
    public float speedz = 0f;

    public AudioSource sound;
    public AudioSource sound2;


    public float min_intense = 300f;
    public float max_intense = 300f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lightbulb.intensity = Random.Range(min_intense, max_intense);
        lightbulb.spotAngle = Random.Range(49f, 51f);

        float x = Mathf.Sin(Time.time * speedx) * rotx;
        float z = Mathf.Cos(Time.time * speedz) * rotz;

        transform.localEulerAngles = new Vector3(x, 0, z);
    }

    public void lightson()
    {
        sound.Play();
        lightbulb.gameObject.SetActive(true);
        outer_light.gameObject.SetActive(true);
    }
    public void lightsoff()
    {
        sound2.Play();
        lightbulb.gameObject.SetActive(false);
        outer_light.gameObject.SetActive(false);
    }
}
