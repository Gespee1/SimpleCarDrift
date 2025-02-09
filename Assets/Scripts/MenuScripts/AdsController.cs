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

    // ������������� �� ������� �������� ������� � OnEnable
    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    // ������������ �� ������� �������� ������� � OnDisable
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    // ����������� ����� ��������� �������
    void Rewarded(int id)
    {
        // ���� ID = 1, �� ����� ������
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
    // ����� ��� ������ ����� �������
    public void ExampleOpenRewardAd(int id)
    {
        // �������� ����� �������� ����� �������
        YandexGame.RewVideoShow(id);
    }*/
}
