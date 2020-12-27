using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchFoodState", menuName = "ScriptableObjects/Brain/SearchFoodState")]
public class SearchFoodState : MovementState
{
	private CarrotController _foodTarget;
	private bool _hasCheckedArea;

	public override void Begin(Brain brain)
	{
		base.Begin(brain);

		brain.TargetFood = null;

		_hasCheckedArea = false;

		onNewTarget += OnNewTarget;
	}

	public override void End(Brain brain)
	{
		onNewTarget -= OnNewTarget;
	}

	public override void Tick(Brain brain)
	{
		if (!brain.Movement.PositionReached()) return;
		_hasCheckedArea |= false; // No effect if the area was already checked
		base.Tick(brain);
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.Movement.PositionReached() && !_hasCheckedArea)
		{
			Debug.Log("Stop at position to find food");
			_hasCheckedArea = true;
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
				Debug.Log("Food found");
				return Brain.Action.ReachFood;
			}
		}
		return brain.CurrentAction;
	}

	#region [Callbacks]

	private void OnNewTarget(object sender, EventArgs data)
	{
		_hasCheckedArea = false;
	}

	#endregion
}
