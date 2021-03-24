using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningCheckpoint : MonoBehaviour
{

	private TextMeshProUGUI warningText;
	private float flashSpeed = 3;
	private IEnumerator currentFlashCoroutine;

	private void Start()
	{
		warningText = GetComponent<TextMeshProUGUI>();
	}

	public void WrongCheckpointWarning()
	{
		//prevent start while running
		if (currentFlashCoroutine == null)
		{
			currentFlashCoroutine = FlashText();
			StartCoroutine(currentFlashCoroutine);
		}

	}


	//flashing warning text
	IEnumerator FlashText()
	{
		//no flahes
		for (int i = 0; i < 7; i++)
		{

			if (warningText.alpha < 0.5)
			{
				for (float a = 0; a < 1; a += Time.deltaTime * flashSpeed)
				{
					warningText.alpha = a;
					yield return null;
				}
			}
			if (warningText.alpha > 0.5)
			{
				for (float a = 1; a > 0; a -= Time.deltaTime * flashSpeed)
				{
					warningText.alpha = a;
					yield return null;
				}
			}

			warningText.alpha = 0f;
		}

		currentFlashCoroutine = null;
	}
}
