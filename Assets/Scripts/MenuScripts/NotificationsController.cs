using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class NotificationsController : MonoBehaviour
{


    private void Awake()
    {
        // Вызов вопроса об отзыве, после 1 сыгранной игры
        if(YandexGame.savesData.firstRideDone
            && YandexGame.EnvironmentData.reviewCanShow)
        {
            YandexGame.ReviewShow(false);
        }

        // Вызов вопроса об ярлыке на раб. стол после 2 сыгранных игр
        if(YandexGame.savesData.secondRideDone
            && YandexGame.EnvironmentData.promptCanShow)
        {
            YandexGame.PromptShow();
        }
    }
}
