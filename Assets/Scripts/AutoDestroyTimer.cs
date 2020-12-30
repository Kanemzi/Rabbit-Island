using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyTimer : MonoBehaviour
{
	public float TimeBeforeDestroy = 5.0f;

	public void Start()
	{
		Destroy(gameObject, TimeBeforeDestroy);
	}
}
