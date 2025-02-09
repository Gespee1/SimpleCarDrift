using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using YG;

public class DayNightController : MonoBehaviour
{
    [SerializeField] DirectionalLight DayLight;
    [SerializeField] DirectionalLight NightLight;


    void Start()
    {
        if (YandexGame.savesData.isDay)
        {
            
        }
        else
        {  

        }
    }

}
