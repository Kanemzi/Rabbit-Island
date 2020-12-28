using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchMateState", menuName = "ScriptableObjects/Brain/SearchMateState")]
public class SearchMateState : MovementState
{
	private CarrotController _foodTarget;
	private bool _hasCheckedArea;

	public override void Begin(Brain brain)
	{
		base.Begin(brain);

		brain.TargetMate = null;

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
		if (_hasCheckedArea)
			brain.CenterPosition = brain.transform.position;

		base.Tick(brain);
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.Hungry) // Find food is the priority
			return Brain.Action.SearchFood;
		else if (brain.TargetMate != null) // If another mate found while searching
			return Brain.Action.WaitMate;

		if (brain.Movement.PositionReached() && !_hasCheckedArea)
		{
			Debug.Log("Stop at position to find mate");
			_hasCheckedArea = true;
			List<RabbitController> potentialPartners = brain.Eyes.GetRabbitsInSight();

			RabbitController closestValidMate = null;
			float minDistance = float.MaxValue;

			foreach (RabbitController rabbit in potentialPartners)
			{
				if (rabbit.Grabbable.Grabbed) continue;
				if (rabbit.Brain == brain) continue; // Can't auto mate !
				if (!rabbit.ReadyToMate || !rabbit.FreeToMate) continue;

				float distance = (brain.transform.position - rabbit.transform.position).sqrMagnitude;
				if (distance < minDistance && brain.Movement.CanReachPosition(rabbit.transform.position))
				{
					closestValidMate = rabbit;
					minDistance = distance;
				}
			}

			if (closestValidMate)
			{
				brain.TargetMate = closestValidMate;
				closestValidMate.Brain.TargetMate = brain.GetComponent<RabbitController>();
				Debug.Log("Mate found");
				return Brain.Action.JoinMate;
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
