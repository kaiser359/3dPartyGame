using UnityEngine;

public class musical_chairs_light : MonoBehaviour
{
    public Light lightbulb;

    public float rotx = 10f;
    public float rotz = 10f;
    public float speedx = 10f;
    public float speedz = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lightbulb.intensity = Random.Range(49f, 51f);
        lightbulb.spotAngle = Random.Range(99f, 101f);

        float x = Mathf.Sin(Time.time * speedx) * rotx;
        float z = Mathf.Cos(Time.time * speedz) * rotz;
        print(Time.time * rotx);

        transform.localEulerAngles = new Vector3(x, 0, z);
    }
}
