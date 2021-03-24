using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	private float defTimeScale;
	private float defFixedDelta;

	private void Start()
	{
		defTimeScale = Time.timeScale;
		defFixedDelta = Time.fixedDeltaTime;
	}

	public void LoadScene(string scenename)
	{
		Time.timeScale = defTimeScale;
		Time.fixedDeltaTime = defFixedDelta;
		SceneManager.LoadScene(scenename);
	}

	public void EndApplication()
	{
		Application.Quit();
	}

	public void ReloadCurrent()
	{
		Time.timeScale = defTimeScale;
		Time.fixedDeltaTime = defFixedDelta;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
