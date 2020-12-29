using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitMateState", menuName = "ScriptableObjects/Brain/WaitMateState")]
public class WaitMateState : BrainState
{
	public override void Begin(Brain brain)
	{
		Debug.Log(brain.GetInstanceID() + " Waiting for mate");
		brain.DoGiveBirth = false;

		brain.Movement.StopMove();
		brain.MateReached = false;
		brain.Movement.LookAtTarget(brain.TargetMate.transform.position);

		brain.TargetMate.onDead += OnTargetCancelled;
		brain.TargetMate.Brain.onChangeAction += OnTargetCancelled;
		brain.TargetMate.Grabbable.onGrab += OnTargetCancelled;
	}

	public override void End(Brain brain)
	{
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
		if (brain.TargetCancelled) return Brain.Action.Idle;
		
		if (brain.MateReached)
			return Brain.Action.Mate;

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

			if (brain)
			{
				brain.TargetCancelled = true;
				UnbindMate(brain);
				brain.TargetMate = null;
			}
		}
	}

	#endregion
}
