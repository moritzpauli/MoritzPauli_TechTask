using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	[SerializeField]
	private Material passedMaterial;

	private MeshRenderer[] modelRenderer = new MeshRenderer[3];

	public bool bReached;
	public float timeReached;
	public Pose carPose;

	private void Start()
	{
		for (int i = 0; i < modelRenderer.Length; i++)
		{
			modelRenderer[i] = transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>();
		}
	}

	public void PassCheckpoint()
	{
		for(int i = 0; i < modelRenderer.Length; i++)
		{
			modelRenderer[i].material = passedMaterial;
		}
	}
}
