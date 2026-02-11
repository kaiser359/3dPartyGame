using UnityEngine;

public class PresentationButton : MonoBehaviour
{
    public GameObject[] goArr;

    private void Start() {
		for (int i = 0; i < goArr.Length; i++)
		{
			goArr[i].SetActive(false);
		}
	}

    private void OnTriggerEnter(Collider col)
    {
        for (int i = 0; i < goArr.Length; i++) { 
            goArr[i].SetActive(true);
        }
    }
}
