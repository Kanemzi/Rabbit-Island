using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchFoodState", menuName = "ScriptableObjects/Brain/SearchFoodState")]
public class SearchFoodState : MovementState
{
	public override void Begin(Brain brain)
	{
		base.Begin(brain);

		brain.TargetFood = null;
		brain.HasCheckedArea = false;

		onNewTarget += OnNewTarget;
	}

	public override void End(Brain brain)
	{
		onNewTarget -= OnNewTarget;
	}

	public override void Tick(Brain brain)
	{
		if (!brain.Movement.PositionReached()) return;
		brain.HasCheckedArea |= false; // No effect if the area was already checked
		base.Tick(brain);
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.Movement.PositionReached() && !brain.HasCheckedArea)
		{
			brain.HasCheckedArea = true;
			List<CarrotController> carrots = brain.Eyes.GetCarrotsInSight();

			CarrotController closestCarrot = null;
			float minDistance = float.MaxValue;

			foreach (CarrotController carrot in carrots)
			{
				if (carrot.Grabbable.Grabbed) continue;

				float distance = (brain.transform.position - carrot.transform.position).sqrMagnitude;
				if (distance < minDistance && brain.Movement.CanReachPosition(carrot.transform.position))
				{
					closestCarrot = carrot;
					minDistance = distance;
				}
			}

			if (closestCarrot)
			{
				brain.TargetFood = closestCarrot;
				return Brain.Action.ReachFood;
			}
		}
		return brain.CurrentAction;
	}

	#region [Callbacks]

	private void OnNewTarget(object sender, EventArgs data)
	{
		if (sender is Movement movement) { 
			movement.GetComponent<Brain>().HasCheckedArea = false;
		}
	}

	#endregion
}
