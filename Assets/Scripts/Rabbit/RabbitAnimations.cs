using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAnimations : MonoBehaviour
{
	[Header("References")]
	public RabbitData Data;

	[SerializeField] private GameObject _body;

	private float _scaling;

	private void Start()
	{
		_scaling = Data.MaxGrowScale - Data.MinGrowScale;
		UpdateScale(0.0f);
	}

	public void OnGrow(object sender, EventArgs data)
	{
		if (sender is RabbitController rabbit)
			UpdateScale(rabbit.LifePercent);
	}

	public void UpdateScale(float growPercent)
	{
		float newScale = Data.MinGrowScale + _scaling * growPercent;
		transform.localScale = Vector3.one * newScale;
	}
}
