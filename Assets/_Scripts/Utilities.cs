using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
	public static string TimeToString(float time)
	{
		string timeString = Mathf.RoundToInt(((int)time / 60)).ToString("D2") + "." + Mathf.RoundToInt((time % 60)).ToString("D2") + "." + (Mathf.RoundToInt(100 * (time % 1))).ToString("D2");
		return timeString;
	}
}
