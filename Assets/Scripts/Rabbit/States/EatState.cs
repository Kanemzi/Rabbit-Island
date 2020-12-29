using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EatState", menuName = "ScriptableObjects/Brain/EatState")]
public class EatState : BrainState
{
    public float TimeToEat = 1f;

	public override void Begin(Brain brain)
	{
		brain.EatTime = TimeToEat;

		brain.Stomach.Eat(brain.TargetFood.FoodSource);
	}

	public override void End(Brain brain)
	{
	}

	public override void Tick(Brain brain)
	{
		brain.EatTime -= Time.deltaTime;
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.EatTime <= 0.0f)
		{
			if (brain.Stomach.IsHungry) return Brain.Action.SearchFood;
			else return Brain.Action.Idle;
		}

		return brain.CurrentAction;
	}
}
