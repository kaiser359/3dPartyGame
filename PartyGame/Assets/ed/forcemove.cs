using UnityEngine;

public class forcemove : MonoBehaviour
{
    public float speed = 10f;
    public GameObject mover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(7f, 9f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
