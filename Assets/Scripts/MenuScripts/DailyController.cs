using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class DailyController : MonoBehaviour
{
    [SerializeField] Button[] dailyButtons;
    [SerializeField] TextMeshProUGUI timeToRewardLeftText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] SelectController selectController;

    private long _serverTime;
    private DateTime serverDateTime, lastDateTime;
    private bool _canGetPrize = false;
    private int _nextDailyRewId = 0;


    private void Start()
    {

        _serverTime = YandexGame.ServerTime();
        //serverDateTime = new DateTime(_serverTime);
        serverDateTime = DateTime.UtcNow; // Временное решение
        lastDateTime = new DateTime(YandexGame.savesData.lastRewardGotTime);


        Debug.Log($"{serverDateTime.ToShortDateString()} {serverDateTime.ToShortTimeString()}");
        Debug.Log($"{lastDateTime.ToShortDateString()} {lastDateTime.ToShortTimeString()}");

        // Чистка ежедневок
        /*for (int i = 0; i < dailyRew.Length; i++)
        {
            dailyRew[i] = false;
        }*/
        setButtonColors();


        //YandexGame.savesData.lastRewardGotTime = _serverTime;
        //YandexGame.savesData.lastRewardGotTime = DateTime.Now.AddHours(-10).AddMinutes(-15).Ticks; // Временное решение
        //YandexGame.SaveProgress();


        StartCoroutine(RewardTimeLeftUpdate());
    }

    private void setButtonColors()
    {
        bool[] dailyRew;

        dailyRew = YandexGame.savesData.dailyRewardGot;
        for (int i = 0; i < dailyRew.Length - 1; i++)
        {
            if (dailyRew[i])
            {
                _nextDailyRewId = i + 1;
                dailyButtons[i].GetComponent<Image>().color = UnityEngine.Color.red;
            }
            else
            {
                break;
            }
        }
    }


    private IEnumerator RewardTimeLeftUpdate()
    {
        while (true)
        {
            UpdateRewardTimeLeft();
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateRewardTimeLeft()
    {
        string timeLeft;
        TimeSpan timeSpan;

        timeSpan = DateTime.UtcNow - lastDateTime;
        // Если последний вход был больше 24ч назад
        if (timeSpan.TotalHours > 24)
        //if (timeSpan.TotalSeconds > 15)
        {
            _canGetPrize = true;
            timeLeft = "00:00:00";
        }
        else // Меньше 24ч назад
        {
            timeSpan = lastDateTime.AddHours(24) - DateTime.UtcNow;
            timeLeft = timeSpan.TotalSeconds > 0 ? $"{(int)timeSpan.TotalHours:D2}:{(int)timeSpan.Minutes:D2}:{(int)timeSpan.Seconds:D2}" : "24:00:00";

            timeToRewardLeftText.text = $"До получения следующей награды осталось: {timeLeft}";
        }

        switch (YandexGame.lang)
        {
            case "ru":
                timeToRewardLeftText.text = $"До получения следующей награды осталось: {timeLeft}";
                break;
            case "tr":
                timeToRewardLeftText.text = $"Bir sonraki ödüle {timeLeft} kaldı";
                break;
            default:
                timeToRewardLeftText.text = $"{timeLeft} left until next reward";
                break;
        }
    }


    private void Update()
    {
        if (_canGetPrize)
        {
            dailyButtons[_nextDailyRewId].interactable = true;
        }
    }


    public void getPrize(int prizeId)
    {
        _canGetPrize = false;
        dailyButtons[_nextDailyRewId].interactable = false;

        switch (prizeId)
        {
            case 0:
                addMoney(1000);
                break;
            case 1:
                addMoney(3000);
                break;
            case 2:
                getDailyCar(51);
                break;
            case 3:
                addMoney(5000);
                break;
            case 4:
                addMoney(7000);
                break;
            case 5:
                getDailyCar(52);
                break;
        }

        lastDateTime = DateTime.UtcNow;
        YandexGame.savesData.dailyRewardGot[prizeId] = true;
        YandexGame.savesData.lastRewardGotTime = lastDateTime.Ticks;
        YandexGame.SaveProgress();

        setButtonColors();
    }


    private void addMoney(int money)
    {
        YandexGame.savesData.money += money;
        YandexGame.SaveProgress();

        moneyText.text = $"{YandexGame.savesData.money}$";
    }

    private void getDailyCar(int _dailyCarIndex)
    {
        selectController.SaveCarPurch(_dailyCarIndex);
    }

}
