using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using static YG.ViewingAdsYG;

/// <summary>
/// Base class game controller.
/// </summary>
public class GameController :MonoBehaviour
{
	//[SerializeField] KeyCode NextCarKey = KeyCode.N;
	[SerializeField] UnityEngine.UI.Button RespawnCarButton;
    [SerializeField] UnityEngine.UI.Button PauseButton;
	[SerializeField] GameObject PausePanel;
	[SerializeField] GameObject StartPanel;
	[SerializeField] GameObject EndPanel;
    [SerializeField] Transform SpawnPoint;
    [SerializeField] GameObject[] cars;
    [SerializeField] Material[] colors;
    [SerializeField] GameObject[] rims;
    [SerializeField] Material lightsMaterial;
	[SerializeField] SettingsController settingsController;

    public static GameController Instance;
	public static CarController PlayerCar { get { return Instance.m_PlayerCar; } }
	public static bool RaceIsStarted { get { return true; } }
	public static bool RaceIsEnded { get { return false; } }

    CarController m_PlayerCar;
	//List<CarController> Cars = new List<CarController>();
	int CurrentCarIndex = 0;
	int colorIndex;
    int rimIndex;
    private List<GameObject> selectedRimsList = new List<GameObject>();


    // Подписываемся на событие открытия/закрытия вкладки игры
    private void OnEnable()
    {
        YandexGame.onVisibilityWindowGame += OnVisibilityWindowGame;
		YandexGame.CloseFullAdEvent += onFullAdClose;
    }

    // Отписываемся от события открытия/закрытия вкладки игры
    private void OnDisable()
    {
        YandexGame.onVisibilityWindowGame -= OnVisibilityWindowGame;
        YandexGame.CloseFullAdEvent -= onFullAdClose;
    }

    protected virtual void Awake ()
	{
		Instance = this;
        GameObject currentWheel;

        CurrentCarIndex = YandexGame.savesData.selectedCar < cars.Length ? YandexGame.savesData.selectedCar : 0;
        m_PlayerCar = Instantiate(cars[CurrentCarIndex], SpawnPoint.position, new Quaternion(), SpawnPoint).GetComponent<CarController>();

		colorIndex = YandexGame.savesData.carColors[CurrentCarIndex];
        m_PlayerCar.GetComponentInChildren<MeshRenderer>().SetMaterials(new List<Material> { colors[colorIndex], lightsMaterial });

        rimIndex = YandexGame.savesData.carRims[CurrentCarIndex];
        foreach (Wheel rim in m_PlayerCar.Wheels)
        {
            selectedRimsList.Add(rim.WheelView.GetChild(0).gameObject);
        }
        for (int i = 0; i < selectedRimsList.Count; i++)
        {
            currentWheel = selectedRimsList[i];

            foreach (Transform child in currentWheel.transform)
            {
                Destroy(child.gameObject);
            }

            Instantiate(rims[rimIndex], currentWheel.transform.position, new Quaternion(), currentWheel.transform);
        }


        if (RespawnCarButton)
        {
            RespawnCarButton.onClick.AddListener (RespawnCar);
		}
		if(PauseButton)
		{
			PauseButton.onClick.AddListener(GamePause);
        }
    }

	void Update () 
	{ 
		if (Input.GetKeyDown (KeyCode.R))
		{
            RespawnCar();
		}
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            GamePause();
        }
    }

	public void GamePause()
	{
		if(Time.timeScale == 0)
		{
			PausePanel.SetActive(false);
			Time.timeScale = 1;
			settingsController.unMuteAll();
		}
		else
		{
			PausePanel.SetActive (true);
            settingsController.muteAll();
            Time.timeScale = 0;
		}
	}

	public void ExitToMenu()
	{
        Time.timeScale = 1;
		//SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(0);
	}

	public void RestartLevel()
    {
        EndPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartCoroutine(wait());
        /*EndPanel.SetActive(false);
        RespawnCar();
        Time.timeScale = 1;
        StartPanel.SetActive(true);
		Time.timeScale = 0;*/
    }

	private void RespawnCar()
	{
		m_PlayerCar.RB.velocity = Vector3.zero;
        m_PlayerCar.transform.position = SpawnPoint.position;
		m_PlayerCar.transform.rotation = SpawnPoint.rotation;
    }


	IEnumerator wait()
	{
        EndPanel.SetActive(false);
        RespawnCar();
        Time.timeScale = 1;

		yield return new WaitForSeconds(0.6f);

        StartPanel.SetActive(true);
        Time.timeScale = 0;
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
	private void onFullAdClose()
	{
		Time.timeScale = 0;
		Debug.Log("game ad close");
	}
}
