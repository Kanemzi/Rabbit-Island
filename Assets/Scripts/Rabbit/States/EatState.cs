using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EatState", menuName = "ScriptableObjects/Brain/EatState")]
public class EatState : BrainState
{
    public float TimeToEat = 1f;

	private float _eatTime;

	public override void Begin(Brain brain)
	{
		_eatTime = TimeToEat;

		brain.Stomach.Eat(brain.TargetFood.FoodSource);
	}

	public override void End(Brain brain)
	{
	}

	public override void Tick(Brain brain)
	{
		_eatTime -= Time.deltaTime;
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (_eatTime <= 0.0f)
		{
			if (brain.Stomach.IsHungry) return Brain.Action.SearchFood;
			else return Brain.Action.Idle;
		}

		return brain.CurrentAction;
	}
}
