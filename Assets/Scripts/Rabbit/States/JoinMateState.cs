using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JoinMateState", menuName = "ScriptableObjects/Brain/JoinMateState")]
public class JoinMateState : BrainState
{
	public virtual void Begin(Brain brain)
	{
		Debug.Log("Joining mate");
		brain.MateReached = false;
	}

	public virtual void End(Brain brain)
	{

	}

	public virtual void Tick(Brain brain)
	{

	}

	public virtual Brain.Action TakeDecision(Brain brain)
	{
		if (brain.MateReached)
			return Brain.Action.Mate;
		return brain.CurrentAction;
	}
}
