using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class TeleportonCollide : MonoBehaviour
{
    public GameObject col;
    public NavMeshSurface surface;

    private void OnTriggerEnter(Collider other)
    {
        float x =  Random.Range(surface.navMeshData.sourceBounds.min.x, surface.navMeshData.sourceBounds.max.x);
        float y = Random.Range(surface.navMeshData.sourceBounds.min.y, surface.navMeshData.sourceBounds.max.y);
        float z = Random.Range(surface.navMeshData.sourceBounds.min.z, surface.navMeshData.sourceBounds.max.z);

        col.transform.position = new Vector3(x, y, z) + surface.navMeshData.position;
    }
}
