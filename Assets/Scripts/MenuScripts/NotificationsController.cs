using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class NotificationsController : MonoBehaviour
{


    private void Awake()
    {
        // ����� ������� �� ������, ����� 1 ��������� ����
        if(YandexGame.savesData.firstRideDone
            && YandexGame.EnvironmentData.reviewCanShow)
        {
            YandexGame.ReviewShow(false);
        }

        // ����� ������� �� ������ �� ���. ���� ����� 2 ��������� ���
        if(YandexGame.savesData.secondRideDone
            && YandexGame.EnvironmentData.promptCanShow)
        {
            YandexGame.PromptShow();
        }
    }
}
