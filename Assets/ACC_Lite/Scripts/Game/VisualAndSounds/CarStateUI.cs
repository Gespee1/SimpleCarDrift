using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// Only visual on UI logic.
/// Speedometer, tachometer and current gear information.
/// </summary>
public class CarStateUI :MonoBehaviour
{

	[SerializeField] int UpdateFrameCount = 3;
	[SerializeField] TextMeshProUGUI SpeedText;
	[SerializeField] TextMeshProUGUI CurrentGearText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI CurrentScoreText;
    [SerializeField] TextMeshProUGUI ScoreMultiplierText;

    [SerializeField] RectTransform TahometerArrow;
	[SerializeField] float MinArrowAngle = 0;
	[SerializeField] float MaxArrowAngle = -315f;

	private RectTransform rectTransScoreMultiText;

	int CurrentFrame;
	CarController SelectedCar { get { return GameController.PlayerCar; } }

    private void Start()
    {
        rectTransScoreMultiText = ScoreMultiplierText.GetComponent<RectTransform>();
    }

    private void Update ()
	{

		if (CurrentFrame >= UpdateFrameCount)
		{
			UpdateGamePanel ();
			CurrentFrame = 0;
		}
		else
		{
			CurrentFrame++;
		}

		UpdateArrow ();
	}

	void UpdateArrow ()
	{
		var procent = SelectedCar.EngineRPM / SelectedCar.GetMaxRPM;
		var angle = (MaxArrowAngle - MinArrowAngle) * procent + MinArrowAngle;
		TahometerArrow.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	void UpdateGamePanel ()
	{
		Vector3 ScoreTextPosition;
		Quaternion ScoreRotation;


		SpeedText.text = SelectedCar.SpeedInHour.ToString ("000");

		switch(SelectedCar.CurrentGear)
		{
			case 0:
				CurrentGearText.text = "N";
				break;
			case -1:
				CurrentGearText.text = "R";
				break;
			default:
                CurrentGearText.text = SelectedCar.CurrentGear.ToString();
				break;
        }
		ScoreText.text = SelectedCar.GetScore.ToString();
		CurrentScoreText.text = SelectedCar.GetCurrentScore.ToString();
		ScoreMultiplierText.text = $"x{SelectedCar.GetScoreMultiplier}";

		rectTransScoreMultiText.anchoredPosition = new Vector2(IntLength(SelectedCar.GetCurrentScore) * 35, 
			rectTransScoreMultiText.anchoredPosition.y);
    }

    public static int IntLength(int i)
    {
        if (i < 0) throw new ArgumentOutOfRangeException();

        if (i == 0)
            return 1;

        return (int)Math.Floor(Math.Log10(i)) + 1;
    }
}
