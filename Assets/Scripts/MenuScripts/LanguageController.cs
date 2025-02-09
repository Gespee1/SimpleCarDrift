using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class LanguageController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textSoonUpdates;
    [SerializeField] TextMeshProUGUI textStartPanel;
    [SerializeField] TextMeshProUGUI textHowTo;

    private void Start()
    {
        switch (YandexGame.lang)
        {
            case "ru":
                if(textSoonUpdates != null)
                    textSoonUpdates.text = "1. Больше карт\n2. Неоновая подсветка\n3. Система увеличения счета при дрифте около препятствий";
                if (textStartPanel != null)
                    textStartPanel.text = "Дрифти, пока не вышло время!\nНе врезайся!\nКаждые 25 очков равны 1$.\nУдачи!";
                if (textHowTo != null)
                    textHowTo.text = "WASD   - Управление машиной\nПробел - Ручной тормоз\n'C'           - Сменить камеру\n'R'           - Восстановить машину\n'ESC'     - Меню";
                break;
            case "tr":
                if (textSoonUpdates != null)
                    textSoonUpdates.text = "1. Daha fazla kart\n2. Neon aydınlatma \n3. Engellerin yakınında sürüklenirken skoru artırma sistemi";
                if (textStartPanel != null)
                    textStartPanel.text = "Zaman dolmadan sürüklenin!\nçarpma!\nher 25 puan 1 $ 'a eşittir.\niyi şanslar!";
                if (textHowTo != null)
                    textHowTo.text = "WASD - Araba kontrolü \n Boşluk çubuğu - El freni\n'c' - Kamerayı değiştir\n'r' - Arabayı geri yükle\n'ESC' - Menü";
                break;
            default:
                if (textSoonUpdates != null)
                    textSoonUpdates.text = "1. More maps \n2. Neon lighting \n3. The system of increasing the score when drifting near obstacles";
                if (textStartPanel != null)
                    textStartPanel.text = "Drift before the time runs out!\n Don't crash!\nEvery 25 points is equal to $1.\nGood luck!";
                if (textHowTo != null)
                    textHowTo.text = "WASD - Car control \nSpace bar - Handbrake\n'C'           - Change the camera\n'R'           - Restore the machine\n'ESC'     - Menu";
                break;
        }
    }

    static public string strOnlyFromDaily()
    {
        string strRet;

        switch (YandexGame.lang)
        {
            case "ru":
                strRet = "Только из ежедневных наград!";
                break;
            case "tr":
                strRet = "Sadece günlük ödüllerden!";
                break;
            default:
                strRet = "Only from daily rewards!";
                break;
        }

        return strRet;
    }

    static public string strFrom()
    {
        string strRet;

        switch (YandexGame.lang)
        {
            case "ru":
                strRet = "из";
                break;
            case "tr":
                strRet = "içinde";
                break;
            default:
                strRet = "from";
                break;
        }

        return strRet;
    }

    static public string strColor()
    {
        string strRet;

        switch (YandexGame.lang)
        {
            case "ru":
                strRet = "Цвет";
                break;
            case "tr":
                strRet = "Renk";
                break;
            default:
                strRet = "Color";
                break;
        }

        return strRet;
    }

    static public string strRim()
    {
        string strRet;

        switch (YandexGame.lang)
        {
            case "ru":
                strRet = "Диск";
                break;
            case "tr":
                strRet = "Jant";
                break;
            default:
                strRet = "Rim";
                break;
        }

        return strRet;
    }

    static public string strMap(int mapIndex)
    {
        string strRet;

        string[] strRu = { "Гоночный трек №1", "Гоночный трек №2", "Город" };
        string[] strTr = { "Yarış pisti # 1", "Yarış pisti # 2", "Şehir" };
        string[] strEn = { "Race Track No. 1", "Race Track No. 2", "City" };

        switch (YandexGame.lang)
        {
            case "ru":
                strRet = strRu[mapIndex];
                break;
            case "tr":
                strRet = strTr[mapIndex];
                break;
            default:
                strRet = strEn[mapIndex];
                break;
        }

        return strRet;
    }
}
