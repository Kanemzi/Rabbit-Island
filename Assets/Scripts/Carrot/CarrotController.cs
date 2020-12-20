using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
	public event EventHandler<GrowData> onGrow;
	public event EventHandler onGrowEnd;
    public event EventHandler onRot;

	public enum GrowState
	{
		Growing, Ripe, Rotten
	}

	public class GrowData : EventArgs
	{
		public float GrowPercent;
		public float RipePercent;
		public GrowState State;
	}

	[Header("References")]
	public CarrotData Data;
	public FoodSource FoodSource;
	public CarrotSpread Spread;
	public Grabbable Grabbable;

	private float _growTime;
	private float _rotTime;

	private bool _growing;
	private float _lifetime;
	private GrowState _growState;

	private void Start()
	{
		onGrow += FoodSource.OnGrow;
		onGrow += Spread.OnGrow;

		Grabbable.onGrab += OnGrab;
		Grabbable.onDrop += OnDrop;

		_growTime = Data.RandomGrowTime;
		_rotTime = _growTime + Data.RandomRotTime;

		_lifetime = 0.0f;
		_growState = GrowState.Growing;

		ResumeGrowing();
	}

	private void Update()
	{
		if (!_growing) return;

		_lifetime += Time.deltaTime;

		float growPercent = _lifetime / _rotTime;
		
		float ripePercent = 0.0f;
		if (_growState == GrowState.Ripe)
		{
			ripePercent = (_lifetime - _growTime) / (_rotTime - _growTime);
		}

		switch (_growState) 
		{
			case GrowState.Growing when _lifetime >= _growTime:
				_growState = GrowState.Ripe;
				onGrowEnd?.Invoke(this, EventArgs.Empty);
				return;

			case GrowState.Ripe when _lifetime >= _rotTime:
				_growState = GrowState.Rotten;
				onRot?.Invoke(this, EventArgs.Empty);
				return;
		}

		onGrow?.Invoke(this, new GrowData
		{
			GrowPercent = growPercent,
			RipePercent = ripePercent,
			State = _growState
		});
	}

	private void ResumeGrowing()
	{
		_growing = true;
		Spread.enabled = true;
	}

	private void PauseGrowing()
	{
		_growing = false;
		Spread.enabled = false;
	}

	private void OnGrab(object sender, Grabbable.GrabData data) => PauseGrowing();
	private void OnDrop(object sender, Grabbable.DropData data) => ResumeGrowing();
}
