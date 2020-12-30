using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JoinMateState", menuName = "ScriptableObjects/Brain/JoinMateState")]
public class JoinMateState : BrainState
{
	public float MaxMateDistance = 0.1f;

	public override void Begin(Brain brain)
	{
		brain.DoGiveBirth = true;
		brain.TargetReached = brain.MateReached = brain.TargetCancelled = false;
		
		brain.Movement.ReachPosition(brain.TargetMate.transform.position);
		brain.Movement.onTargetReached += OnTargetReached;

		brain.TargetMate.onDead += OnTargetCancelled;
		brain.TargetMate.Brain.onChangeAction += OnTargetCancelled;
		brain.TargetMate.Grabbable.onGrab += OnTargetCancelled;
	}

	public override void End(Brain brain)
	{
		brain.Movement.onTargetReached -= OnTargetReached;

		UnbindMate(brain);
	}

	public override void Tick(Brain brain)
	{
		if (brain.TargetMate && brain.TargetMate.Brain.TargetMate == null)
		{
			brain.TargetCancelled = true;
		}
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.TargetCancelled) return Brain.Action.SearchMate;
		if (brain.TargetReached) {
			if (brain.MateReached)
				return Brain.Action.Mate;
			else return Brain.Action.SearchMate;
		}
		return brain.CurrentAction;
	}

	private void UnbindMate(Brain brain)
	{
		if (!brain.TargetMate) return;
		brain.TargetMate.onDead -= OnTargetCancelled;
		brain.TargetMate.Brain.onChangeAction -= OnTargetCancelled;
		brain.TargetMate.Grabbable.onGrab -= OnTargetCancelled;
	}

	#region [Callbacks]

	private void OnTargetCancelled(object sender, EventArgs data)
	{
		if (sender is Brain brain)
		{
			if (brain.CurrentAction == Brain.Action.WaitMate || brain.CurrentAction == Brain.Action.Mate)
			{
				return;
			}
		}

		if (sender is MonoBehaviour behaviour)
		{
			brain = behaviour.GetComponent<Brain>();

			if (!brain.TargetMate) return;
			if (behaviour is Grabbable) brain = brain.TargetMate.Brain;
			
			if (brain) {
				brain.TargetCancelled = true;
				UnbindMate(brain);
				brain.TargetMate = null;
			}
		}
	}

	private void OnTargetReached(object sender, EventArgs data)
	{
		if (sender is Movement movement)
		{
			Brain brain = movement.GetComponent<Brain>();
			if (!brain.TargetMate)
			{
				brain.TargetCancelled = true;
				return;
			}

			brain.TargetReached = true;
			float distance = (movement.transform.position - movement.Target).sqrMagnitude;
			if (distance > MaxMateDistance * MaxMateDistance) return;

			brain.MateReached = brain.TargetMate.Brain.MateReached = true;
		}
	}

	#endregion
}
