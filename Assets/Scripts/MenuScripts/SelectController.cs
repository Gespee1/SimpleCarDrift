using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class SelectController : MonoBehaviour
{
    [SerializeField] GameObject[] cars;
    [SerializeField] Texture2D[] maps;
    //[SerializeField] string[] mapNames;
    [SerializeField] Material[] colors;
    [SerializeField] GameObject[] rims;
    [SerializeField] Material lightsMaterial;
    [SerializeField] CarPurchInfo[] carPurchInfo;
    [SerializeField] Button prevCarButton;
    [SerializeField] Button nextCarButton;
    [SerializeField] Button firstCarButton;
    [SerializeField] Button lastCarButton;
    [SerializeField] Button prevMapButton;
    [SerializeField] Button nextMapButton;
    [SerializeField] Button prevColorButton;
    [SerializeField] Button nextColorButton;
    [SerializeField] Button prevRimButton;
    [SerializeField] Button nextRimButton;
    [SerializeField] Button startButton;
    [SerializeField] Button buyButton;
    [SerializeField] Button buyColorButton;
    [SerializeField] Button buyRimButton;
    [SerializeField] Button tuningButton;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI carNameText;
    [SerializeField] TextMeshProUGUI mapNameText;
    [SerializeField] TextMeshProUGUI carCostText;
    [SerializeField] TextMeshProUGUI colorText;
    [SerializeField] TextMeshProUGUI rimText;
    [SerializeField] Image mapImage;
    [SerializeField] GameObject carCostLockPanel;
    [SerializeField] GameObject carDonateLockPanel;
    [SerializeField] int defaultMoney;
    [SerializeField] bool resetProgress = false;

    private const string moneyTag = "money";
    private const string selectedCarTag = "selectedCar";
    private const string highscoreTag = "highscore";
    private GameObject selectedCar;
    private List<GameObject> selectedRimsList = new List<GameObject>();
    private Texture2D selectedMap;
    private int selectedCarIndex;
    private int selectedMapIndex;
    private int selectedColorIndex;
    private int selectedRimIndex;
    private int _money;
    private int _highscore;

    private CarInfo _carInfo;
    private PurchaseYG _purchYG;

    // Подписываемся на событие открытия/закрытия вкладки игры
    private void OnEnable()
    {
        YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;
    }

    // Отписываемся от события открытия/закрытия вкладки игры
    private void OnDisable()
    {
        YandexGame.onVisibilityWindowGame -= OnVisibilityWindowGame;
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (resetProgress)
        {
            YandexGame.ResetSaveProgress();
        }
        //_money = PlayerPrefs.HasKey(moneyTag) ? PlayerPrefs.GetInt(moneyTag) : defaultMoney;
        _purchYG = carDonateLockPanel.GetComponentInChildren<PurchaseYG>();
        _money = YandexGame.savesData.money;
        moneyText.text = $"{_money} $";

        //_highscore = PlayerPrefs.HasKey(highscoreTag) ? PlayerPrefs.GetInt(highscoreTag) : 0;
        _highscore = YandexGame.savesData.highscore;
        highscoreText.text = $"{_highscore}";

        selectedMapIndex = 0;

        selectedCarIndex = YandexGame.savesData.selectedCar == 0 ? PlayerPrefs.HasKey(selectedCarTag) ? PlayerPrefs.GetInt(selectedCarTag) : 0 : YandexGame.savesData.selectedCar;
        //selectedColorIndex = PlayerPrefs


        for(int i = 0; i < carPurchInfo.Length; i++)
            carPurchInfo[i].Initialize(i);


        SpawnSelectedCar(selectedCarIndex);
        ShowSelectedMap(selectedMapIndex);
    }

    public void PrevCar()
    {
        nextCarButton.SetActive(true);

        DeleteCurrentCar();
        selectedCarIndex -= 1;
        SpawnSelectedCar(selectedCarIndex);
    }

    public void NextCar()
    {
        prevCarButton.SetActive(true);

        DeleteCurrentCar();
        selectedCarIndex += 1;
        SpawnSelectedCar(selectedCarIndex);
    }

    public void FirstCar()
    {
        prevCarButton.SetActive(true);

        DeleteCurrentCar();
        selectedCarIndex = 0;
        SpawnSelectedCar(selectedCarIndex);
    }

    public void LastCar()
    {
        prevCarButton.SetActive(true);

        DeleteCurrentCar();
        selectedCarIndex = carPurchInfo.Length - 1;
        SpawnSelectedCar(selectedCarIndex);
    }

    private void DeleteCurrentCar()
    {
        if (selectedCar != null)
        {
            Destroy(selectedCar);
        }
    }

    private void SpawnSelectedCar(int carIndex)
    {
        firstCarButton.interactable = prevCarButton.interactable = carIndex > 0;
        lastCarButton.interactable = nextCarButton.interactable = carIndex < cars.Length - 1;

        selectedCar = cars[carIndex];

        if (selectedCar != null)
        {
            selectedCar = Instantiate(selectedCar, transform.position, new Quaternion(), transform);
            selectedCar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            selectedCar.GetComponent<CarSoundController>().enabled = false;
            selectedCar.GetComponent<CarController>().enabled = false;
            selectedCar.GetComponent<UserControl>().enabled = false;
            selectedCar.GetComponent<AudioListener>().enabled = false;
            foreach(AudioSource source in selectedCar.GetComponentsInChildren<AudioSource>())
            {
                source.enabled = false;
            }

            buyButton.SetActive(!carPurchInfo[carIndex].IsPurchased && carPurchInfo[carIndex].type == CarPurchType.defaultType);
            startButton.SetActive(carPurchInfo[carIndex].IsPurchased);
            tuningButton.SetActive(carPurchInfo[carIndex].IsPurchased);
            carCostLockPanel.SetActive(!carPurchInfo[carIndex].IsPurchased && carPurchInfo[carIndex].type != CarPurchType.donate);
            carDonateLockPanel.SetActive(!carPurchInfo[carIndex].IsPurchased && carPurchInfo[carIndex].type == CarPurchType.donate);


            switch (carPurchInfo[carIndex].type)
            {
                case CarPurchType.defaultType:
                    carCostText.text = $"{carPurchInfo[carIndex].Cost} $";
                    break;
                case CarPurchType.daily:
                    carCostText.text = LanguageController.strOnlyFromDaily();
                    break;
                case CarPurchType.donate:
                    if(!carPurchInfo[carIndex].IsPurchased)
                    {
                        _purchYG.data = YandexGame.PurchaseByID(carPurchInfo[carIndex].donateId);
                        _purchYG.UpdateEntries();
                    }
                    break;
            }
            
            _carInfo = selectedCar.GetComponent<CarInfo>();
            carNameText.text = $"{_carInfo.CarName} ({carIndex+1} {LanguageController.strFrom()} {cars.Length})";

            selectedColorIndex = carPurchInfo[selectedCarIndex].CarColor;
            ShowSelectedColor(selectedColorIndex);

            findRimsContainers();
            selectedRimIndex = carPurchInfo[selectedCarIndex].CarRim;
            ShowSelectedRim(selectedRimIndex);
        }
    }

    public void buyCar()
    {
        _money = YandexGame.savesData.money;

        if (_money >= carPurchInfo[selectedCarIndex].Cost)
        {
            _money -= carPurchInfo[selectedCarIndex].Cost;
            carPurchInfo[selectedCarIndex].savePurchase(selectedCarIndex);
            YandexGame.savesData.money = _money;
            YandexGame.SaveProgress();

            moneyText.text = $"{_money} $";
            buyButton.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
            startButton.SetActive(carPurchInfo[selectedCarIndex].IsPurchased);
            tuningButton.SetActive(carPurchInfo[selectedCarIndex].IsPurchased);
            carCostLockPanel.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
            carDonateLockPanel.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
        }
    }

    public void PrevMap()
    {
        nextMapButton.SetActive(true);

        selectedMapIndex -= 1;
        ShowSelectedMap(selectedMapIndex);
    }

    public void NextMap()
    {
        prevMapButton.SetActive(true);

        selectedMapIndex += 1;
        ShowSelectedMap(selectedMapIndex);
    }

    private void ShowSelectedMap(int mapIndex)
    {
        prevMapButton.interactable = mapIndex > 0;
        nextMapButton.interactable = mapIndex < maps.Length - 1;

        selectedMap = maps[mapIndex];

        if (selectedMap != null)
        {
            mapImage.GetComponent<Image>().sprite = Sprite.Create(selectedMap, new Rect(0.0f, 0.0f, selectedMap.width, selectedMap.height), new Vector2(0.5f, 0.5f)/*, 100.0f*/);

            mapNameText.text = LanguageController.strMap(selectedMapIndex);
        }
    }

    public void PrevColor()
    {
        nextColorButton.SetActive(true);

        selectedColorIndex -= 1;
        ShowSelectedColor(selectedColorIndex);
    }

    public void NextColor()
    {
        prevColorButton.SetActive(true);

        selectedColorIndex += 1;
        ShowSelectedColor(selectedColorIndex);
    }

    private void ShowSelectedColor(int colorIndex)
    {
        prevColorButton.interactable = colorIndex > 0;
        nextColorButton.interactable = colorIndex < colors.Length - 1;
        buyColorButton.SetActive(colorIndex != carPurchInfo[selectedCarIndex].CarColor);

        selectedCar.GetComponentInChildren<MeshRenderer>().SetMaterials(new List<Material> { colors[colorIndex], lightsMaterial });
        colorText.text = $"{LanguageController.strColor()} №{colorIndex + 1}";
    }

    public void BuySelectedColor()
    {
        carPurchInfo[selectedCarIndex].saveColor(selectedCarIndex, selectedColorIndex);

        buyColorButton.SetActive(false);
    }
    public void BuySelectedRim()
    {
        if(_money >= 500)
        {
            _money -= 500;
            carPurchInfo[selectedCarIndex].saveRim(selectedCarIndex, selectedRimIndex);
            YandexGame.savesData.money = _money;
            YandexGame.SaveProgress();

            moneyText.text = $"{_money} $";

            buyRimButton.SetActive(false);
        }
    }

    private void ShowSelectedRim(int rimIndex)
    {
        GameObject currentWheel;

        prevRimButton.interactable = rimIndex > 0;
        nextRimButton.interactable = rimIndex < rims.Length - 1;
        buyRimButton.SetActive(rimIndex != carPurchInfo[selectedCarIndex].CarRim);

        for (int i = 0; i < selectedRimsList.Count; i++)
        {
            currentWheel = selectedRimsList[i];

            foreach (Transform child in currentWheel.transform)
            {
                Destroy(child.gameObject);
            }

            Instantiate(rims[selectedRimIndex], currentWheel.transform.position, new Quaternion(), currentWheel.transform);
        } 

        rimText.text = $"{LanguageController.strRim()} №{rimIndex + 1}";
    }

    public void PrevRim()
    {
        nextRimButton.SetActive(true);

        selectedRimIndex -= 1;
        ShowSelectedRim(selectedRimIndex);
    }

    public void NextRim()
    {
        prevRimButton.SetActive(true);

        selectedRimIndex += 1;
        ShowSelectedRim(selectedRimIndex);
    }

    private void findRimsContainers()
    {
        CarController carController;
        
        if(selectedRimsList.Count > 0)
            selectedRimsList.Clear();

        carController = selectedCar.GetComponent<CarController>();
        if (carController != null)
        {
            foreach (Wheel rim in carController.Wheels)
            {
                selectedRimsList.Add(rim.WheelView.GetChild(0).gameObject);
            }
        }
    }

    public void ExitTuning()
    {
        selectedColorIndex = carPurchInfo[selectedCarIndex].CarColor;
        ShowSelectedColor(selectedColorIndex);
        selectedRimIndex = carPurchInfo[selectedCarIndex].CarRim;
        ShowSelectedRim(selectedRimIndex);
    }




    // Сохранение выбранного авто и загрузка уровня
    public void StartGame()
    {
        //PlayerPrefs.SetInt("selectedCar", selectedCarIndex);
        //PlayerPrefs.SetInt("selectedColor", selectedColorIndex);

        YandexGame.savesData.selectedCar = selectedCarIndex;
        YandexGame.SaveProgress();

        switch(selectedMapIndex)
        {
            case 0:
                SceneManager.LoadScene(selectedMapIndex + 1);
                break;
            case 1:
                SceneManager.LoadScene(selectedMapIndex + 1);
                break;
            case 2:
                SceneManager.LoadScene(selectedMapIndex + 1);
                break;
            default:
                Debug.Log("Отсутствует сцена для загрузки");
                break;
        }
    }

    private void OnVisibilityWindowGame(bool focus)
    {
        if (focus)
        {
            YandexGame.GameplayStart();
        }
        else
        {
            YandexGame.GameplayStop();

        }
    }



    public void SaveCarPurch(int _carIndex)
    {
        carPurchInfo[_carIndex].savePurchase(_carIndex);

        buyButton.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
        startButton.SetActive(carPurchInfo[selectedCarIndex].IsPurchased);
        tuningButton.SetActive(carPurchInfo[selectedCarIndex].IsPurchased);
        carCostLockPanel.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
        carDonateLockPanel.SetActive(!carPurchInfo[selectedCarIndex].IsPurchased);
    }


}

[System.Serializable]
class CarPurchInfo
{
    [SerializeField] string _key;

    public bool IsPurchased = false;
    public CarPurchType type;
    public string donateId;
    public int Cost;
    public int CarColor;
    public int CarRim;

    public void Initialize(int CarIndex)
    {
        IsPurchased = YandexGame.savesData.carPurchases[CarIndex];
        CarColor = YandexGame.savesData.carColors[CarIndex];
        CarRim = YandexGame.savesData.carRims[CarIndex];
    }

    public void saveColor(int CarIndex, int colorIdx)
    {
        this.CarColor = colorIdx;
        YandexGame.savesData.carColors[CarIndex] = colorIdx;
        YandexGame.SaveProgress();
    }

    public void saveRim(int CarIndex, int rimIdx)
    {
        this.CarRim = rimIdx;
        YandexGame.savesData.carRims[CarIndex] = rimIdx;
        YandexGame.SaveProgress();
    }

    public void savePurchase(int CarIndex)
    {
        IsPurchased = true;

        YandexGame.savesData.carPurchases[CarIndex] = true;
        YandexGame.SaveProgress();
    }

}


public enum CarPurchType
{
    defaultType,
    daily,
    donate
}
