using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using TMPro;

public class GameSaver : MonoBehaviour
{
	private  string datapath;
	private HighscoreSave saveFile;

	[SerializeField]
	private TextMeshProUGUI nameText;


	private void Awake()
	{
		datapath = Application.persistentDataPath + "/gamesave.save";
		if (nameText)
			nameText.text = PlayerPrefs.GetString("Name");
	}



	public void ChangeName(string name)
	{
		PlayerPrefs.SetString("Name", name);
	}


	//loads savefile and saves new information
	public void SaveScore(string course, float scoreTime)
	{

		saveFile = new HighscoreSave();
		saveFile = LoadScore();
		//check if player exists
		if (saveFile.players.Count > 0)
		{
			PlayerProfile playerMatch = saveFile.players.FirstOrDefault(i => i.playerName == PlayerPrefs.GetString("Name"));
			if (playerMatch != null)
			{
				CourseScore courseMatch = playerMatch.courses.FirstOrDefault(c => c.name == course);
				if (courseMatch != null)
				{
					if (courseMatch.time > scoreTime)
					{
						courseMatch.time = scoreTime;
					}
				}
				else
				{
					CourseScore newS = new CourseScore(course, scoreTime);
					playerMatch.courses.Add(newS);
				}

			}
			else
			{
				PlayerProfile newP = new PlayerProfile(PlayerPrefs.GetString("Name"));
				CourseScore newS = new CourseScore(course, scoreTime);
				newP.courses.Add(newS);
				saveFile.players.Add(newP);
			}
		}
		else
		{
			PlayerProfile newP = new PlayerProfile(PlayerPrefs.GetString("Name"));
			CourseScore newS = new CourseScore(course, scoreTime);
			newP.courses.Add(newS);
			saveFile.players.Add(newP);
		}

		foreach (PlayerProfile player in saveFile.players)
		{
			player.CalculateAverage();
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream sFile = File.Create(datapath);
		bf.Serialize(sFile, saveFile);
		sFile.Close();
	}


	//loads deserializes returns savefile
	public HighscoreSave LoadScore()
	{
		HighscoreSave hSave = new HighscoreSave();
		if (File.Exists(datapath))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream sFile = File.Open(datapath, FileMode.Open);
			hSave = bf.Deserialize(sFile) as HighscoreSave;
			sFile.Close();
		}	
		return hSave;
	}
}
