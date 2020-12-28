using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MateState", menuName = "ScriptableObjects/Brain/MateState")]
public class MateState : BrainState
{
	public virtual void Begin(Brain brain)
	{

	}

	public virtual void End(Brain brain)
	{
		brain.TargetMate = null;
	}

	public virtual void Tick(Brain brain)
	{

	}

	public virtual Brain.Action TakeDecision(Brain brain)
	{
		return brain.CurrentAction;
	}
}
