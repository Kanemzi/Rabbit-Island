using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAnimations : MonoBehaviour
{
	[Header("References")]
	public RabbitData Data;

	[SerializeField] private GameObject _body;
	[SerializeField] private Animator _animator;

	private float _scaling;

	private void Start()
	{
		_scaling = Data.MaxGrowScale - Data.MinGrowScale;
		UpdateScale(0.0f);
	}

	public void OnGrow(object sender, EventArgs data)
	{
		Debug.Log("Grow");
		if (sender is RabbitController rabbit)
			UpdateScale(rabbit.LifePercent);
	}

	public void UpdateScale(float growPercent)
	{
		float newScale = Data.MinGrowScale + _scaling * growPercent;
		transform.localScale = Vector3.one * newScale;
	}

	public void OnRabbitGrabbed(object sender, EventArgs data)
	{
		_animator.SetBool("Grabbed", true);
	}

	public void OnRabbitDropped(object sender, EventArgs data)
	{
		_animator.SetBool("Grabbed", false);
	}

	public void OnRabbitStartMove(object sender, EventArgs data)
	{
		Debug.Log("Start move");
		_animator.SetBool("Moving", true);
	}

	public void OnRabbitStopMove(object sender, EventArgs data)
	{
		Debug.Log("Stop move");
		_animator.SetBool("Moving", false);
	}

	public void OnRabbitEat(object sender, EventArgs data)
	{
		_animator.SetTrigger("Eat");
	}

	public void OnRabbitMate(object sender, EventArgs data)
	{
		_animator.SetTrigger("Mate");
	}
}
