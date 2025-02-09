using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PurchaseShopController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI donate30Text,
                                     donate100Text;
    [SerializeField] ImageLoadYG donate30Image,
                                 donate100Image;
    private void OnEnable()
    {
        YG.Utils.Pay.Purchase purch;

        purch = YandexGame.PurchaseByID("donate30");
        if (purch != null)
        {
            donate30Text.text = purch.priceValue;
            donate30Image.urlImage = purch.currencyImageURL;
            donate30Image.gameObject.GetComponent<RawImage>().SetActive(true);
            donate30Image.Load(purch.currencyImageURL);
        }
        purch = YandexGame.PurchaseByID("donate100");
        if (purch != null)
        {
            donate100Text.text = purch.priceValue;
            donate100Image.urlImage = purch.currencyImageURL;
            donate100Image.gameObject.GetComponent<RawImage>().SetActive(true);
            donate100Image.Load(purch.currencyImageURL);
        }
    }
}
