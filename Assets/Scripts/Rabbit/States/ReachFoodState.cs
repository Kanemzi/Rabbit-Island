using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReachFoodState", menuName = "ScriptableObjects/Brain/ReachFoodState")]
public class ReachFoodState : BrainState
{
	public float MaxEatDistance = 0.1f;

	public override void Begin(Brain brain)
	{
		brain.TargetReached = brain.FoodReached = brain.TargetCancelled = false;
		
		brain.Movement.ReachPosition(brain.TargetFood.transform.position);
		brain.Movement.onTargetReached += OnTargetReached;

		// Link events to cancel (food moved, eaten, etc...)
		brain.TargetFood.onRot += OnTargetCancelled;
		brain.TargetFood.FoodSource.onEaten += OnTargetCancelled;
		brain.TargetFood.Grabbable.onGrab += OnTargetCancelled;

		brain.TargetFood.TargetedBy.Add(brain.GetComponent<RabbitController>());
	}

	public override void End(Brain brain)
	{
		brain.TargetFood.TargetedBy.Remove(brain.GetComponent<RabbitController>());

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
		if (brain.TargetCancelled) return Brain.Action.SearchFood;
		if (brain.TargetReached)
		{
			if (brain.FoodReached) return Brain.Action.Eat;
			else return Brain.Action.SearchFood;
		}

		return brain.CurrentAction;
	}

	#region [Callbacks]

	private void OnTargetCancelled(object sender, EventArgs data)
	{
		if (sender is MonoBehaviour behaviour) 
		{
			CarrotController carrot = behaviour.GetComponent<CarrotController>();
			foreach(RabbitController rabbit in carrot.TargetedBy)
			{
				Brain brain = rabbit.GetComponent<Brain>();
				Debug.Log("CANCEL for food : " + brain);

				if (brain)
				{
					Debug.Log("True cancel");
					brain.TargetCancelled = true;
				}
			}
		}
	}

	private void OnTargetReached(object sender, EventArgs data)
	{
		if (sender is Movement movement)
		{
			Brain brain = movement.GetComponent<Brain>();
			brain.TargetReached = true;
			float distance = (movement.transform.position - movement.Target).sqrMagnitude;
			if (distance > MaxEatDistance * MaxEatDistance) return;

			brain.FoodReached = true;
			// Debug.Log("FOOD !");
		}
	}

	#endregion
}
