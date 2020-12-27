using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObjects/Brain/IdleState")]
public class IdleState : MovementState
{
	public override void Begin(Brain brain)
	{
		base.Begin(brain);
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
		return brain.CurrentAction;
	}
}
