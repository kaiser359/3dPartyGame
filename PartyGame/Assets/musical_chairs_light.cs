using UnityEngine;

public class musical_chairs_light : MonoBehaviour
{
    public Light lightbulb;

    public float rotx = 10f;
    public float rotz = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lightbulb.intensity = Random.Range(49f, 51f);
        lightbulb.spotAngle = Random.Range(99f, 101f);

        rotx = Mathf.Sin(Time.time * rotx);
        rotz = Mathf.Sin(Time.time * rotz);
        print(Time.time);

        transform.localEulerAngles = new Vector3(rotx, 0, rotz);
    }
}
