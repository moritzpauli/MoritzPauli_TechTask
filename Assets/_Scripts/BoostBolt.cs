using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBolt : MonoBehaviour
{
    [SerializeField]
    private float rotSpeed;

	[SerializeField]
	private float boostAmount;

	[SerializeField]
	private MeshRenderer boltRenderer;

	private ParticleSystem collectEffect;
	

	private void Start()
	{
		collectEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
		collectEffect.Stop();
	}

	private void Update()
	{
		transform.Rotate(transform.up, rotSpeed*Time.deltaTime);
	}



	public float CollectBoost()
	{
		collectEffect.Play();
		StartCoroutine(FadeBolt());
		return boostAmount;	
	}


	//visually fades out bolt and destroys it afterwards
	IEnumerator FadeBolt()
	{
		for(float a = 1; a > 0; a -= Time.deltaTime * 2)
		{
			Color boltC = boltRenderer.material.color;
			boltC.a = a;
			boltRenderer.material.color = boltC;
			yield return null;
		}
		
		Destroy(gameObject);
	}
	
}
