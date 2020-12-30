using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RabbitAnimations : MonoBehaviour
{
	[Header("References")]
	public Stomach Stomach;
	public RabbitData Data;
	public GameObject MateVFXPrefab;
	public VisualEffect WantToMateVFX;
	public VisualEffect HungryVFX;

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
		WantToMateVFX.Stop();
		HungryVFX.gameObject.SetActive(false);
	}

	public void OnRabbitDropped(object sender, EventArgs data)
	{
		_animator.SetBool("Grabbed", false);
		if (Stomach.IsHungry)
			HungryVFX.gameObject.SetActive(true);
	}

	public void OnRabbitStartMove(object sender, EventArgs data)
	{
		_animator.SetBool("Moving", true);
	}

	public void OnRabbitStopMove(object sender, EventArgs data)
	{
		_animator.SetBool("Moving", false);
	}

	public void OnRabbitEat(object sender, EventArgs data)
	{
		_animator.SetTrigger("Eat");
	}

	public void OnRabbitMate(object sender, EventArgs data)
	{
		_animator.SetTrigger("Mate");
		Instantiate(MateVFXPrefab, transform.position, Quaternion.identity);
	}

	public void OnBrainChangeAction(object sender, EventArgs data)
	{
		if (sender is Brain brain)
		{
			if (brain.CurrentAction == Brain.Action.SearchMate
				|| brain.CurrentAction == Brain.Action.WaitMate)
			{
				WantToMateVFX.Play();
			}
			else if (brain.CurrentAction == Brain.Action.Idle
				|| brain.CurrentAction == Brain.Action.SearchFood)
			{
				WantToMateVFX.Stop();
			}
		}
	}

	public void OnRabbitHungry(object sender, EventArgs data)
	{
		HungryVFX.gameObject.SetActive(true);
	}

	public void OnRabbitReplete(object sender, EventArgs data)
	{
		HungryVFX.gameObject.SetActive(false);
	}
}
