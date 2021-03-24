using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIScoreboard : MonoBehaviour
{
	[SerializeField]
	private GameObject scoreboard;

	[SerializeField]
	private TextMeshProUGUI[] scoresText = new TextMeshProUGUI[10];

	public List<PlayerProfile> highPlayers = new List<PlayerProfile>();

	private HighscoreSave saveData;
	private GameSaver loader;
	private string space = "        ";

	private void Start()
	{
		loader = GetComponent<GameSaver>();
		saveData = loader.LoadScore();
		PopulateBoard();

	}

	
	private void PopulateBoard()
	{
		highPlayers.Clear();
		highPlayers.AddRange(saveData.players);
		highPlayers = highPlayers.OrderBy(o => o.avgTime).ToList();
		if (highPlayers.Count > 10)
		{
			highPlayers.RemoveRange(9, highPlayers.Count - 9);
		}

		for (int i = 0; i < highPlayers.Count; i++)
		{
			scoresText[i].text = highPlayers[i].playerName + space;
			for (int c = 0; c < highPlayers[i].courses.Count; c++)
			{
				string courses = highPlayers[i].courses[c].name + ": " + Utilities.TimeToString(highPlayers[i].courses[c].time)+ space;
				scoresText[i].text += courses;
			}
		}
	}
}
