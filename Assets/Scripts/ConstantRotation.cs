using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
	public float Speed;

	private void Update()
	{
		transform.RotateAround(Vector3.zero, Vector3.up, Speed * Time.deltaTime);
	}
}