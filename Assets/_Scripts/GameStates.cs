using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStates : MonoBehaviour
{

	[SerializeField]
	private CarController playerCar;

	private GameSaver scoreSaver;

	//ui
	[SerializeField]
	private TextMeshProUGUI timerText;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private TextMeshProUGUI[] startNumbers;
	[SerializeField]
	private GameObject finishScreen;
	[SerializeField]
	private TextMeshProUGUI finishTime;
	[SerializeField]
	private TextMeshProUGUI recordTime;
	[SerializeField]
	private Transform starContainer;
	[SerializeField]
	private Transform dashStarContainer;




	public bool bPaused;

	private bool bStarted;
	public float timer;
	private float defTimeScale;
	private float defFixedDelta;

	[SerializeField]
	private float[] starRequirements;


	private void Start()
	{
		defTimeScale = Time.timeScale;
		defFixedDelta = Time.fixedDeltaTime;
		scoreSaver = GetComponent<GameSaver>();
		StartCoroutine(Countdown());
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
			if (!bPaused)
				PauseGame();
			else
				ResumeGame();
		}

		if (bStarted)
		{
			timer += Time.deltaTime;
			timerText.text = Utilities.TimeToString(timer);
			//switch off stars
			for (int i = 0; i < dashStarContainer.childCount; i++)
			{
				if (timer > starRequirements[i])
				{
					dashStarContainer.GetChild(i).gameObject.GetComponent<StarBehaviour>().DeductStar();
				}
			}
		}
	}


	public void PauseGame()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.0f;
		bPaused = true;
	}

	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = defTimeScale;
		Time.fixedDeltaTime = defFixedDelta;
		bPaused = false;
	}

	public void FinishRace()
	{
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.0f;
		finishScreen.SetActive(true);
		scoreSaver.SaveScore(SceneManager.GetActiveScene().name, timer);
		//current time
		finishTime.text = Utilities.TimeToString(timer);
		//read record
		HighscoreSave save = scoreSaver.LoadScore();
		for (int i = 0; i < save.players.Count; i++)
		{
			if (save.players[i].playerName == PlayerPrefs.GetString("Name"))
			{
				for (int c = 0; c < save.players[i].courses.Count; c++)
				{
					if (save.players[i].courses[c].name == SceneManager.GetActiveScene().name)
					{
						recordTime.text = "Your Highscore: " + Utilities.TimeToString(save.players[i].courses[c].time);
					}
				}
			}
		}
		//switch on stars
		for (int i = 0; i < starRequirements.Length; i++)
		{
			if (timer < starRequirements[i])
			{
				starContainer.GetChild(i).gameObject.SetActive(true);
			}
		}

	}


	IEnumerator Countdown()
	{
		for (int i = 0; i < 4; i++)
		{
			startNumbers[i].gameObject.SetActive(true);
			for (float a = 1; a > 0; a -= Time.deltaTime * 0.8f)
			{
				startNumbers[i].alpha = a;
				startNumbers[i].rectTransform.localScale = new Vector3(8 - a * 7, 8 - a * 7, 1);
				yield return null;
			}
			startNumbers[i].gameObject.SetActive(false);
		}

		bStarted = true;
		playerCar.bStarted = true;
	}
}
