using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitController : MonoBehaviour
{
	public event EventHandler onGrow;
	public event EventHandler onDead; // age or starve

	[Header("References")]
    public RabbitData Data;
    public Brain Brain;
    public Stomach Stomach;
	public Grabbable Grabbable;
	public Movement Movement;
	public NavMeshAgent Agent;
	public RabbitAnimations Animations;

	private float _deathAge;

	private bool _growing;
	private float _currentAge;

	public float LifePercent => _currentAge / _deathAge;
	public bool ReadyToMate =>
		Data.MinMateAge <= _currentAge
		&& _currentAge <= Data.MaxMateAge
		&& Stomach.FillPercent >= Data.MinStomachFillForMate;

	private void Awake()
	{
		_deathAge = Data.RandomDeathAge;
		_currentAge = 0.0f;
		_growing = true;

		// Binds events
		onGrow += Stomach.OnRabbitGrow;
		onGrow += Movement.OnRabbitGrow;
		onGrow += Animations.OnGrow;

		Grabbable.onGrab += OnGrab;
		Grabbable.onDrop += OnDrop;
		Grabbable.onDrop += Brain.OnDrop;

		Stomach.onHungry += Brain.OnRabbitHungry;
		Stomach.onReplete += Brain.OnRabbitReplete;
	}

	private void Update()
	{
		if (!_growing) return;

		_currentAge += Time.deltaTime;

		onGrow?.Invoke(this, EventArgs.Empty);
	}

	public void SetAge(float age)
	{
		_currentAge = age;
		onGrow?.Invoke(this, EventArgs.Empty);
	}

	private void ResumeGrowing()
	{
		_growing = true;
		Stomach.enabled = true;
		Movement.enabled = true;
		Brain.enabled = true;
		Agent.enabled = true;
	}

	private void PauseGrowing()
	{
		_growing = false;
		Stomach.enabled = false;
		Movement.enabled = false;
		Brain.enabled = false;
		Agent.enabled = false;
	}

	private void OnGrab(object sender, Grabbable.GrabData data) => PauseGrowing();
	private void OnDrop(object sender, Grabbable.DropData data) => ResumeGrowing();
}
