using UnityEngine;

public class audiomix : MonoBehaviour
{
    public AudioSource AudioSource;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.pitch = Random.Range(0.8f, 1.2f);
            AudioSource.Play();
        }
    }
}
