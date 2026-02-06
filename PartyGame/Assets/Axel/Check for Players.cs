using UnityEngine;

public class CheckforPlayers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("check collider");

        if (other.transform.parent != null && other.transform.parent.gameObject.TryGetComponent(out MoveTo move)) 
        {
            move.currentstate = MoveTo.State.fleeing;
            Debug.Log("fleeing");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Flee Check failed");

        if (other.transform.parent != null && other.transform.parent.gameObject.TryGetComponent(out MoveTo move))
        {
            move.currentstate = MoveTo.State.normal;
            Debug.Log("normal");
        }
    }

}
