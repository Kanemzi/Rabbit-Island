using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitMateState", menuName = "ScriptableObjects/Brain/WaitMateState")]
public class WaitMateState : BrainState
{
	public virtual void Begin(Brain brain)
	{
		Debug.Log("Waiting for mate");
		brain.Movement.StopMove();
		brain.MateReached = false;
	}

	public virtual void End(Brain brain)
	{

	}

	public virtual void Tick(Brain brain)
	{
		brain.Movement.LookAtTarget(brain.TargetMate.transform.position);
	}

	public virtual Brain.Action TakeDecision(Brain brain)
	{
		if (brain.MateReached)
			return Brain.Action.Mate;
		return brain.CurrentAction;
	}
}
