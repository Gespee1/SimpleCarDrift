using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class AdsController : MonoBehaviour
{
    [SerializeField] private int _money = 3000;
    [SerializeField] TextMeshProUGUI _moneyText;
    [SerializeField] SelectController selectController;

    // Подписываемся на событие открытия рекламы в OnEnable
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    // Отписываемся от события открытия рекламы в OnDisable
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    // Подписанный метод получения награды
    void Rewarded(int id)
    {
        // Если ID = 1, то выдаём деньги
        switch(id)
        {
            case 1:
                AddMoney();
                break;
            case 2:
                selectController.BuySelectedColor();
                break;
        }
    }

    private void AddMoney()
    {
        YandexGame.savesData.money += _money;
        YandexGame.SaveProgress();

        _moneyText.text = $"{YandexGame.savesData.money} $";
    }
    /*
    // Метод для вызова видео рекламы
    public void ExampleOpenRewardAd(int id)
    {
        // Вызываем метод открытия видео рекламы
        YandexGame.RewVideoShow(id);
    }*/
}
