using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotController : MonoBehaviour
{
	public event EventHandler<GrowData> onGrow;
	public event EventHandler onGrowEnd;
    public event EventHandler onRot;
	public event EventHandler onMerge;

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
	public CarrotAnimations Animations;

	private float _growTime;
	private float _rotTime;

	private bool _growing;
	private float _lifetime;
	public GrowState State { get; private set; }

	private void Start()
	{
		onGrow += FoodSource.OnGrow;
		onGrow += Spread.OnGrow;

		Grabbable.onGrab += OnGrab;
		Grabbable.onDrop += OnDrop;

		_growTime = Data.RandomGrowTime;
		_rotTime = _growTime + Data.RandomRotTime;

		onRot += Animations.OnRot;
		onRot += OnRot;
		onGrow += Animations.OnGrow;

		_lifetime = 0.0f;
		State = GrowState.Growing;

		ResumeGrowing();
	}

	private void Update()
	{
		if (!_growing) return;

		_lifetime += Time.deltaTime;

		float growPercent = _lifetime / _rotTime;
		
		float ripePercent = 0.0f;
		if (State == GrowState.Ripe)
		{
			ripePercent = (_lifetime - _growTime) / (_rotTime - _growTime);
		}

		switch (State) 
		{
			case GrowState.Growing when _lifetime >= _growTime:
				State = GrowState.Ripe;
				onGrowEnd?.Invoke(this, EventArgs.Empty);
				return;

			case GrowState.Ripe when _lifetime >= _rotTime:
				State = GrowState.Rotten;
				onRot?.Invoke(this, EventArgs.Empty);
				return;
		}

		onGrow?.Invoke(this, new GrowData
		{
			GrowPercent = growPercent,
			RipePercent = ripePercent,
			State = State
		});
	}

	public void Merge(CarrotController other) {
		FoodSource.Merge(other.FoodSource);
		Destroy(other.gameObject);

		onMerge?.Invoke(this, EventArgs.Empty);
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
	private void OnRot(object sender, EventArgs data) => PauseGrowing();
	private void OnDrop(object sender, Grabbable.DropData data) => ResumeGrowing();
}
