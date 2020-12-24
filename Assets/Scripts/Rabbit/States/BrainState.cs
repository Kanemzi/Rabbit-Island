using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrainState : ScriptableObject
{
    public virtual void Begin(Brain brain)
	{

	}

	public virtual void End(Brain brain)
	{

	}

	public virtual void Tick(Brain brain)
	{
		
	}

	public virtual Brain.Action TakeDecision(Brain brain)
	{
		return brain.CurrentAction;
	}
}
