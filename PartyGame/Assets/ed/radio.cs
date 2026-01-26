using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    public void stopsong()
    {
        StartCoroutine(stop());
    }

    public IEnumerator stop()
    {
        float timeElapsed = 0;
        current_song.pitch = 1;

        while (timeElapsed < 2)
        {
            current_song.pitch = Mathf.Lerp(1, 0, timeElapsed / 2);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }
        current_song.pitch = 0f;
        while (timeElapsed < 4)
        {
            current_song.volume = Mathf.Lerp(1, 0, timeElapsed / 4);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }
        current_song.Stop();
    }

}
