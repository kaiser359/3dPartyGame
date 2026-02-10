using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class Chance : MonoBehaviour
{
    //public int player1;
    //PlayerInputManager playerInputManager;
    //public PlayerInput playerInput;
    public Tag_Movement1[] gameObjects;
   // public GameObject playerPrefab;

    private void Start()
    {
        int index = Random.Range(0, gameObjects.Length);
        gameObjects = FindObjectsByType<Tag_Movement1>(FindObjectsSortMode.None);
        gameObjects[index].isTagger = true;
    }
    
   


}
