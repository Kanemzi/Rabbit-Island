using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour
{
	public event EventHandler onGrow;

	[Header("References")]
    public RabbitData Data;
    public Brain Brain;
    public Stomach Stomach;
	public Grabbable Grabbable;
	public Movement Movement;

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

		onGrow += Stomach.OnRabbitGrow;

		Grabbable.onGrab += OnGrab;
		Grabbable.onDrop += OnDrop;
	}

	private void Update()
	{
		if (!_growing) return;

		_currentAge += Time.deltaTime;

		onGrow?.Invoke(this, EventArgs.Empty);
	}

	private void ResumeGrowing()
	{
		_growing = true;
		Stomach.enabled = true;
		Brain.enabled = true;
	}

	private void PauseGrowing()
	{
		_growing = false;
		Stomach.enabled = false;
		Brain.enabled = false;
	}

	private void OnGrab(object sender, Grabbable.GrabData data) => PauseGrowing();
	private void OnDrop(object sender, Grabbable.DropData data) => ResumeGrowing();
}
