using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI finalMoneyText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;

    [SerializeField] TextMeshProUGUI x3multiText;
    [SerializeField] TextMeshProUGUI x2multiText;
    [SerializeField] TextMeshProUGUI x2multiText2;
    [SerializeField] TextMeshProUGUI x15multiText;
    [SerializeField] TextMeshProUGUI x15multiText2;

    [SerializeField] GameObject EndPanel;
    [SerializeField] Image arrowImage;
    [SerializeField] Button multiplierButton;
    [SerializeField] int maxLevelTime = 90;
    [SerializeField] int moneyDemultiplicator = 30;

    private const string moneyTag = "money";
    private const string highscoreTag = "highscore";
    private float _raceStartTime, moneyMultiplier;
    private bool _gamePaused;
    private int _earnedMoney, _extraMoney;


    private void Awake()
    {
        Time.timeScale = 0;
        _raceStartTime = Time.time;
    }

    public void StartRace()
    {
        Time.timeScale = 1;
        _gamePaused = false;

        _raceStartTime = Time.time;
    }

    public void MultiplierAds()
    {
        float xPos = arrowImage.rectTransform.anchoredPosition.x;

        arrowImage.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;

        if (xPos >= -36 && xPos <= 36)
        {
            moneyMultiplier = 2;
            x3multiText.color = Color.red;
            x3multiText.fontSize = 48;
        }
        else if(xPos >= -144 && xPos < -36
            || xPos > 36 && xPos < 144)
        {
            moneyMultiplier = 1;
            if(xPos > 0)
            {
                x2multiText2.color = Color.red;
                x2multiText2.fontSize = 48;
            }
            else
            {
                x2multiText.color = Color.red;
                x2multiText.fontSize = 48;
            }
        }
        else
        {
            moneyMultiplier = 0.5f;
            if (xPos > 0)
            {
                x15multiText2.color = Color.red;
                x15multiText2.fontSize = 48;
            }
            else
            {
                x15multiText.color = Color.red;
                x15multiText.fontSize = 48;
            }
        }

        arrowImage.rectTransform.SetAnchoredX(xPos);

        // показ рекламы
        //YandexGame.RewVideoShow(2);
        multiplierButton.SetActive(false);
    }

    private void rewarded(int id)
    {
        if (id == 2)
            multiplyMoney();
    }

    public void multiplyMoney()
    {
        _extraMoney = Convert.ToInt32(_earnedMoney * moneyMultiplier);
        finalMoneyText.text = $"{_extraMoney + _earnedMoney}$";

        _extraMoney += PlayerPrefs.HasKey("money") ? PlayerPrefs.GetInt("money") : 0;
        PlayerPrefs.SetInt("money", _extraMoney);
        Time.timeScale = 0;
    }



    private void Update()
    {
        int highscore;
        int money = 0;

        timerText.text = (maxLevelTime - Time.time + _raceStartTime).ToStringTime("{0:00}:{1:00}");

        if (Time.time - _raceStartTime > maxLevelTime && !_gamePaused)
        {
            Time.timeScale = 0;
            EndPanel.SetActive( true );

            highscore = Convert.ToInt32(scoreText.text) + Convert.ToInt32(currentScoreText.text);
            _earnedMoney = highscore / moneyDemultiplicator;
            
            finalScoreText.text = highscore.ToString();
            finalMoneyText.text = $"{_earnedMoney}$";

            money = _earnedMoney;
            money += PlayerPrefs.HasKey("money") ? PlayerPrefs.GetInt("money") : 0;
            PlayerPrefs.SetInt("money", money);

            if(PlayerPrefs.HasKey("highscore") 
                && highscore > PlayerPrefs.GetInt("highscore"))
            {
                PlayerPrefs.SetInt("highscore", highscore);

                //YandexGame.NewLeaderboardScores("BestScore", highscore);
            }

            if(!PlayerPrefs.HasKey("firstRideDone"))
            {
                PlayerPrefs.SetInt("firstRideDone", 1);
            }
            else if(!PlayerPrefs.HasKey("secondRideDone"))
            {
                PlayerPrefs.SetInt("secondRideDone", 1);
            }

            _gamePaused = true;
        }
    }
}
