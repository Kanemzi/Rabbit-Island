using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MateState", menuName = "ScriptableObjects/Brain/MateState")]
public class MateState : BrainState
{
	public float TimeToMate = 1.2f;

	public override void Begin(Brain brain)
	{
		brain.MateTime = TimeToMate;
		brain.Reproduction.Mate();

		if (brain.DoGiveBirth)
		{
			Reproduction mate = brain.TargetMate.Reproduction;
			int maxChildren = Mathf.Min(brain.Reproduction.ChildrenLeft, mate.ChildrenLeft);
			int birthCount = Random.Range(1, maxChildren + 1);
			
			brain.Reproduction.GiveBirth(mate, birthCount);
		}
	}

	public override void End(Brain brain)
	{
		brain.TargetMate = null;
		brain.WantToMate = false;
	}

	public override void Tick(Brain brain)
	{
		brain.MateTime -= Time.deltaTime;
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		if (brain.MateTime <= 0.0f)
		{
			if (brain.Stomach.IsHungry) return Brain.Action.SearchFood;
			else return Brain.Action.Idle;
		}

		return brain.CurrentAction;
	}
}
