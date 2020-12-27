using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReachFoodState", menuName = "ScriptableObjects/Brain/ReachFoodState")]
public class ReachFoodState : BrainState
{
	public float MaxEatDistance = 0.1f;

	private bool _targetCancelled;
	private bool _targetReached;
	private bool _foodReached;

	public override void Begin(Brain brain)
	{
		brain.Movement.ReachPosition(brain.TargetFood.transform.position);
		brain.Movement.onTargetReached += OnTargetReached;

		_targetReached = _foodReached = _targetCancelled = false;

		// Link events to cancel (food moved, eaten, etc...)
		brain.TargetFood.onRot += OnTargetCancelled;
		brain.TargetFood.FoodSource.onEaten += OnTargetCancelled;
		brain.TargetFood.Grabbable.onGrab += OnTargetCancelled;
	}

	public override void End(Brain brain)
	{
		brain.Movement.onTargetReached -= OnTargetReached;

		if (brain.TargetFood) {
			brain.TargetFood.onRot -= OnTargetCancelled;
			brain.TargetFood.FoodSource.onEaten -= OnTargetCancelled;
			brain.TargetFood.Grabbable.onGrab -= OnTargetCancelled;
		}
	}

	public override void Tick(Brain brain)
	{
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (_targetCancelled) return Brain.Action.SearchFood;
		if (_targetReached)
		{
			if (_foodReached) return Brain.Action.Eat;
			else return Brain.Action.SearchFood;
		}

		return brain.CurrentAction;
	}

	#region [Callbacks]

	private void OnTargetCancelled(object sender, EventArgs data)
	{
		Debug.Log("CANCEL");
		_targetCancelled = true;
	}

	private void OnTargetReached(object sender, EventArgs data)
	{
		_targetReached = true;

		if (sender is Movement movement)
		{
			float distance = (movement.transform.position - movement.Target).sqrMagnitude;
			if (distance > MaxEatDistance * MaxEatDistance) return;

			_foodReached = true;
			Debug.Log("FOOD !");
		}
	}

	#endregion
}
