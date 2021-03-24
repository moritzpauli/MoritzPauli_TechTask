using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelRequirements")]
public class TrackRequirements : ScriptableObject
{
	public float[] StarThreshold;
	public string Message;
}
