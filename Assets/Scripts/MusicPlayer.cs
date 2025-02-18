using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    private AudioClip GetClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    void Update()
    {
        if(PlayerPrefs.HasKey("music")
            && PlayerPrefs.GetFloat("music") == 0)
        {
            audioSource.Stop();
        }

        if (!audioSource.isPlaying 
            && Time.timeScale != 0
            && PlayerPrefs.HasKey("music")
            && PlayerPrefs.GetFloat("music") != 0)
        {
            audioSource.clip = GetClip();
            audioSource.Play();
        }
    }
}