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
	public Reproduction Reproduction;

	private float _deathAge;

	private bool _growing;
	private float _currentAge;

	public float LifePercent => _currentAge / _deathAge;

	// Is the rabbit in a good state to mate
	public bool ReadyToMate =>
		Data.MinMateAge <= _currentAge
		&& _currentAge <= Data.MaxMateAge
		&& Stomach.FillPercent >= Data.MinStomachFillForMate
		&& Reproduction.ChildrenLeft > 0;

	// Is the rabbit free to mate (false if he's already joining a partner)
	public bool FreeToMate => Brain.TargetMate == null 
		&& (Brain.CurrentAction == Brain.Action.Idle || Brain.CurrentAction == Brain.Action.SearchMate);

	private void Awake()
	{
		_deathAge = Data.RandomDeathAge;
		_currentAge = 0.0f;
		_growing = true;

		// Binds events
		onGrow += Stomach.OnRabbitGrow;
		onGrow += Movement.OnRabbitGrow;
		onGrow += Reproduction.OnRabbitGrow;
		onGrow += Animations.OnGrow;

		Grabbable.onGrab += OnGrab;
		Grabbable.onDrop += OnDrop;
		Grabbable.onDrop += Brain.OnDrop;

		Stomach.onHungry += Brain.OnRabbitHungry;
		Stomach.onReplete += Brain.OnRabbitReplete;
		Reproduction.onWantToMate += Brain.OnWantToMate;
	}

	private void Update()
	{
		if (!_growing) return;

		_currentAge += Time.deltaTime;

		if (_currentAge >= _deathAge) Kill();

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

	public void Kill()
	{
		onDead?.Invoke(this, EventArgs.Empty);
		Destroy(gameObject);
	}

	private void OnGrab(object sender, Grabbable.GrabData data) => PauseGrowing();
	private void OnDrop(object sender, Grabbable.DropData data) => ResumeGrowing();
	private void OnStarve(object sender, EventArgs data) => Kill();
}
