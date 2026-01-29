using UnityEngine;
using UnityEngine.AI;

public class TeleportonCollide : MonoBehaviour
{
    public Vector3 bottomleft, topright;
    public GameObject col;
    private void OnTriggerEnter(Collider other)
    {
        float x = Random.Range(bottomleft.x, topright.x);
        float y = Random.Range(bottomleft.y, topright.y);
        float z = Random.Range(bottomleft.z, topright.z);

        col.transform.position = new Vector3(x, y, z);
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bottomleft, 1);
        Gizmos.DrawWireSphere(topright, 1);
    }
}
