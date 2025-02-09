using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PurchaseController : MonoBehaviour
{
    [SerializeField] SelectController selectController;

    // Подписываемся на ивенты успешной/неуспешной покупки
    private void OnEnable()
    {
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
    }

    private void OnDisable()
    {
        YandexGame.PurchaseSuccessEvent -= SuccessPurchased;
    }

    // Покупка успешно совершена, выдаём товар
    void SuccessPurchased(string id)
    {
        switch(id)
        {
            case "AdsOff":
                YandexGame.savesData.disableAds = true;
                YandexGame.SaveProgress();
                break;
            case "donate30":
                YandexGame.savesData.money += 30000;
                YandexGame.SaveProgress();
                break;
            case "donate100":
                YandexGame.savesData.money += 100000;
                YandexGame.SaveProgress();
                break;
            case "Tesla":
                selectController.SaveCarPurch(44);
                break;
            case "Audi":
                selectController.SaveCarPurch(45);
                break;
            case "Gallardo":
                selectController.SaveCarPurch(46);
                break;
            case "Countach":
                selectController.SaveCarPurch(47);
                break;
            case "Ferrari":
                selectController.SaveCarPurch(48);
                break;
            case "Mclaren":
                selectController.SaveCarPurch(49);
                break;
            case "Agera":
                selectController.SaveCarPurch(50);
                break;
            case "Color":
                selectController.BuySelectedColor();
                break;
        }

    }

}
