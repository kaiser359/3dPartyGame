using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    [SerializeField] float elapsedTime;
    
   public void Start()
    {
        //Time.timeScale = 0f;
    }   


    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            //GameOver();
            timerText.color = Color.red;
            DisableEnablePlayerMovement.instance.DisablePlayerMovement();
        }



        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

