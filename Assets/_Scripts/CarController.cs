using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{

	//wheel colliders
	[SerializeField]
	private WheelCollider[] wheelColliders;
	[SerializeField]
	private WheelConfig[] wheelConfigurations;
	private float forwardStiffness;
	private float sidewayStiffness;

	[System.Serializable]
	public struct WheelConfig
	{
		public bool bMotor;
		public bool bSteer;
	}

	//physics
	private Rigidbody carRb;
	private Vector3 localVel;

	//wheel models
	[SerializeField]
	private Transform[] wheelModels;


	//ui
	[SerializeField]
	private Image boostMeterUI;
	[SerializeField]
	private TextMeshProUGUI speedometer;
	[SerializeField]
	private WarningCheckpoint warning;


	//car specs
	[SerializeField]
	private float maxSteeringAngle;
	[SerializeField]
	private float maxMotorPower;
	[SerializeField]
	private float boostPower;
	[SerializeField]
	private float boostTankMax;

	public float boostTank;
	private bool bBoosting;
	public bool bStarted;


	//input
	private float horizontalInput;


	//effects
	[SerializeField]
	private ParticleSystem exhaustFlame;
	[SerializeField]
	private GameObject exhaustLight;
	[SerializeField]
	private TrailRenderer[] skidMarks;


	//checkpoints,resets
	[SerializeField]
	private Checkpoint[] checkpoints;
	[SerializeField]
	private GameStates gameState;
	private int cpNumber;

	//sound
	private CarAudio audioHandler;


	// Start is called before the first frame update
	void Start()
	{
		carRb = GetComponent<Rigidbody>();
		exhaustFlame.Stop();
		audioHandler = GetComponent<CarAudio>();
		forwardStiffness = wheelColliders[0].forwardFriction.stiffness;
		sidewayStiffness = wheelColliders[0].sidewaysFriction.stiffness;
	}

	private void FixedUpdate()
	{
		if (bStarted)
		{
			localVel = transform.InverseTransformDirection(carRb.velocity);
			RecogniseGroundProp();
			ApplySteering();
			ApplyMotor();
			PositionWheelModels();
			SkidMarks();
			UpdateUI();
		}


	}



	private void RecogniseGroundProp()
	{
		int offTrack = 0;
		for (int i = 0; i < wheelColliders.Length; i++)
		{
			WheelHit hit;
			if (wheelColliders[i].GetGroundHit(out hit))
			{
				if (!(hit.collider.tag is "Tarmack"))
				{
					WheelFrictionCurve fFriction = wheelColliders[i].forwardFriction;
					WheelFrictionCurve sFriction = wheelColliders[i].sidewaysFriction;
					fFriction.stiffness = forwardStiffness * hit.collider.material.staticFriction;
					sFriction.stiffness = sidewayStiffness * hit.collider.material.staticFriction;
					wheelColliders[i].forwardFriction = fFriction;
					wheelColliders[i].sidewaysFriction = sFriction;
					if (!(hit.collider.tag is "Mud"))
					{
						offTrack++;
					}

				}
				else
				{
					WheelFrictionCurve fFriction = wheelColliders[i].forwardFriction;
					WheelFrictionCurve sFriction = wheelColliders[i].sidewaysFriction;
					fFriction.stiffness = forwardStiffness;
					sFriction.stiffness = sidewayStiffness;
					wheelColliders[i].forwardFriction = fFriction;
					wheelColliders[i].sidewaysFriction = sFriction;
				}
			}
			else
			{
				WheelFrictionCurve fFriction = wheelColliders[i].forwardFriction;
				WheelFrictionCurve sFriction = wheelColliders[i].sidewaysFriction;
				fFriction.stiffness = forwardStiffness;
				sFriction.stiffness = sidewayStiffness;
				wheelColliders[i].forwardFriction = fFriction;
				wheelColliders[i].sidewaysFriction = sFriction;
			}
		}


		if (offTrack == 4)
		{
			carRb.drag = 0.2f;
		}
		else
		{
			carRb.drag = 0.05f;
		}
	}

	private void ApplySteering()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		for (int i = 0; i < 4; i++)
		{
			if (wheelConfigurations[i].bSteer)
			{
				wheelColliders[i].steerAngle = maxSteeringAngle * horizontalInput;
			}
		}
	}

	private void ApplyMotor()
	{

		if (boostTank > 0 && (Input.GetKey(KeyCode.Joystick1Button0)||Input.GetKey(KeyCode.Space)))
		{
			boostTank -= Time.deltaTime;
			bBoosting = true;
			exhaustFlame.Play();
			exhaustLight.SetActive(true);

		}
		else
		{
			bBoosting = false;
			exhaustFlame.Stop();
			exhaustLight.SetActive(false);
		}
		for (int i = 0; i < 4; i++)
		{
			if (wheelConfigurations[i].bMotor)
			{
				wheelColliders[i].motorTorque = maxMotorPower * Input.GetAxis("Trigger2");

				if (bBoosting)
				{
					wheelColliders[i].motorTorque += boostPower;

				}
			}
		}


	}

	private void PositionWheelModels()
	{
		for (int i = 0; i < 4; i++)
		{
			Vector3 colPosition;
			Quaternion colRotation;

			wheelColliders[i].GetWorldPose(out colPosition, out colRotation);

			wheelModels[i].position = colPosition;
			wheelModels[i].rotation = colRotation;
		}
	}

	private void UpdateUI()
	{
		boostMeterUI.fillAmount = boostTank / boostTankMax;
		speedometer.text = Mathf.RoundToInt(Mathf.Abs(localVel.z) * 1.6f).ToString() + " km/h";
	}

	private void SkidMarks()
	{
		if (Mathf.Abs(localVel.x) > Mathf.Abs(localVel.z / 3f) && Mathf.Abs(localVel.z) > 11f)
		{
			for (int i = 0; i < 4; i++)
			{
				if (wheelColliders[i].isGrounded)
				{
					skidMarks[i].emitting = true;
					audioHandler.PlayDrift();
				}
				else
				{
					skidMarks[i].emitting = false;
				}

			}
		}
		else
		{
			audioHandler.StopDrift();
			foreach (TrailRenderer trail in skidMarks)
			{
				trail.emitting = false;

			}
		}
	}

	public void ResetCheckPoint()
	{
		if (cpNumber > 0)
		{
			carRb.velocity = Vector3.zero;
			carRb.angularVelocity = Vector3.zero;
			transform.position = checkpoints[cpNumber - 1].carPose.position;
			transform.rotation = checkpoints[cpNumber - 1].carPose.rotation;
			gameState.timer = checkpoints[cpNumber - 1].timeReached;
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boost")
		{
			BoostBolt hitBoost = other.GetComponent<BoostBolt>();
			boostTank += hitBoost.CollectBoost();
		}
		if (other.tag == "Checkpoint")
		{
			if (cpNumber < checkpoints.Length)
			{
				if (other.gameObject.GetInstanceID() == checkpoints[cpNumber].gameObject.GetInstanceID())
				{
					checkpoints[cpNumber].carPose.position = transform.position;
					checkpoints[cpNumber].carPose.rotation = transform.rotation;
					checkpoints[cpNumber].timeReached = gameState.timer;
					checkpoints[cpNumber].PassCheckpoint();
					cpNumber++;
				}
				else
				{
					warning.WrongCheckpointWarning();
				}
			}
		}
		if (other.tag == "Finish")
		{
			if (cpNumber >= checkpoints.Length)
			{
				gameState.FinishRace();
				bStarted = false;
			}
		}
	}
}

