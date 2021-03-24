using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{


	

	private Vector3 carVel;
	private Rigidbody carRb;

	//audiosources
	private AudioSource motor;
	private AudioSource drift;

	//drift parameters
	[SerializeField]
	private float maxVolume;
	[SerializeField]
	private float incVolume;

	//motor parameters
	[SerializeField]
	private float pitchCoefficient;
	[SerializeField]
	private float minPitch;
	[SerializeField]
	private float maxPitch;

	private void Start()
	{
		carRb = GetComponent<Rigidbody>();
		motor = GetComponents<AudioSource>()[0];
		drift = GetComponents<AudioSource>()[1];
		motor.Play();
	}

	private void Update()
	{
		carVel = transform.InverseTransformDirection(carRb.velocity);
		if (Mathf.Abs(carVel.z) > 0)
		{
			motor.volume = 0.9f;
			motor.pitch = minPitch +  carVel.z * pitchCoefficient;
			motor.pitch = Mathf.Clamp(motor.pitch, minPitch, maxPitch);
		}
		if(Time.timeScale == 0f)
		{
			motor.pitch = minPitch;
			motor.volume = 0.6f;
		}
	}

	public void PlayDrift()
	{
		if (!drift.isPlaying)
		{
			drift.Play();
			StartCoroutine(FadeInDrift());
		}
	}

	public void StopDrift()
	{
		StartCoroutine(FadeOutDrift());
	}

	IEnumerator FadeInDrift()
	{

		for (float v = 0; v <= maxVolume; v += Time.deltaTime * incVolume)
		{
			drift.volume = v;
			yield return null;
		}
		

	}

	IEnumerator FadeOutDrift()
	{

		for (float v = maxVolume; v >= 0; v -= Time.deltaTime * incVolume)
		{
			drift.volume = v;
			yield return null;
		}
		drift.volume = 0;
		drift.Stop();

	}
}
