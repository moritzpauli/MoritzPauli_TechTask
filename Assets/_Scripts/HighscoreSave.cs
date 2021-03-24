using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighscoreSave
{
	public List<PlayerProfile> players = new List<PlayerProfile>();
}




[System.Serializable]
public class PlayerProfile
{
	public string playerName;
	public List<CourseScore> courses = new List<CourseScore>();
	public float avgTime;

	public PlayerProfile(string name)
	{
		playerName = name;
	}

	public void CalculateAverage()
	{
		float time = 0;
		for(int i = 0; i < courses.Count; i++)
		{
			time += courses[i].time;
		}
		avgTime = time / courses.Count;
	}
}

[System.Serializable]
public class CourseScore
{
	public string name;
	public float time;

	public CourseScore(string name,float time)
	{
		this.name = name;
		this.time = time;
	}
}
