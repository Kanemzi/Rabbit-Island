using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObjects/Brain/IdleState")]
public class IdleState : MovementState
{
	public override void Begin(Brain brain)
	{
		base.Begin(brain);
		
		if( brain.TargetMate)
		{
			brain.TargetMate.Brain.TargetMate = null;
		}
		brain.TargetMate = null;
		brain.WantToMate = false;
	}

	public override void End(Brain brain)
	{

	}

	public override void Tick(Brain brain)
	{
		base.Tick(brain);
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.Hungry)
			return Brain.Action.SearchFood;
		else if (brain.WantToMate)
			return Brain.Action.SearchMate;
		else if (brain.TargetMate != null)
			return Brain.Action.WaitMate;

		return brain.CurrentAction;
	}

	
}
