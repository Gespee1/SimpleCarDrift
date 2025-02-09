using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = /*FindObjectOfType<AudioSource>()*/GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    private AudioClip GetClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    void Update()
    {
        //Debug.Log(audioSource.isPlaying.ToString());
        if(YandexGame.savesData.music == 0)
        {
            audioSource.Stop();
        }

        if (!audioSource.isPlaying 
            && Time.timeScale != 0
            && YandexGame.savesData.music != 0)
        {
            audioSource.clip = GetClip();
            audioSource.Play();
        }
    }
}