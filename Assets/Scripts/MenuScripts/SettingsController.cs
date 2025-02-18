using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioSource[] audioSrcs;
    [SerializeField] AudioSource[] soundSrcs;
    [SerializeField] GameObject carSpawnPoint;

    private float music;
    private float sound;

    private void Start()
    {
        List<AudioSource> soundSources;
        music = PlayerPrefs.HasKey("music") ? PlayerPrefs.GetFloat("music") : 0;
        sound = PlayerPrefs.HasKey("sound") ? PlayerPrefs.GetFloat("sound") : 0;

        musicSlider.value = music;
        soundSlider.value = sound;

        if(carSpawnPoint != null)
        {
            soundSources = soundSrcs.ToList<AudioSource>();
            foreach (var audioSrc in carSpawnPoint.GetComponentsInChildren<AudioSource>())
            {
                soundSources.Add(audioSrc);
            }
            soundSrcs = soundSources.ToArray();
        }

        foreach (var src in soundSrcs)
            src.volume = sound;

        foreach (var src in audioSrcs)
            src.volume = music;
    }


    public void musicValueChanged()
    {
        PlayerPrefs.SetFloat("music",musicSlider.value);

        foreach (var src in audioSrcs)
            src.volume = musicSlider.value;
    }

    public void soundValueChanged()
    {
        PlayerPrefs.SetFloat("sound", soundSlider.value);

        foreach (var src in soundSrcs)
            src.volume = soundSlider.value;
    }

    public void muteAll()
    {
        foreach (var src in audioSrcs)
            src.Pause();
        foreach (var src in soundSrcs)
            src.Pause();
    }

    public void unMuteAll()
    {
        foreach (var src in audioSrcs)
            src.UnPause();
        foreach (var src in soundSrcs)
            src.UnPause();
    }
}
