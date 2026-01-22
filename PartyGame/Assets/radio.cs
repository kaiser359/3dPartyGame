using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class radio : MonoBehaviour
{
    public List<AudioClip> music = new();
    public AudioSource current_song;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void playsong()
    {
        current_song.clip = music[Random.Range(0, music.Count)];
        current_song.Play();
    }

    // Update is called once per frame
}
