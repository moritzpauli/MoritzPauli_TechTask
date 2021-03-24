using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBehaviour : MonoBehaviour
{

	private Image starImage;

	private void Start()
	{
		starImage = GetComponent<Image>();
	}

	public void DeductStar()
	{
		StartCoroutine(FadeZoom());
	}

	IEnumerator FadeZoom()
	{
		for (float a = 1; a > 0; a -= Time.deltaTime * 1.5f)
		{
			starImage.color = new Color(starImage.color.r, starImage.color.g, starImage.color.b, a);
			starImage.rectTransform.localScale = new Vector3(8 - a * 7, 8 - a * 7, 1);
			yield return null;
		}
		Destroy(gameObject);
	}
}
