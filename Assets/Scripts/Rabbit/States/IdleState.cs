using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "ScriptableObjects/Brain/IdleState")]
public class IdleState : BrainState
{
    public float IdleMovementsRange = 6.0f;
	[MinMaxSlider(0.1f, 16.0f)] public Vector2 MoveTimeInterval;

	public override void Begin(Brain brain)
	{
		Debug.Log("Start idle");
		brain.CenterPosition = brain.transform.position;
		brain.TimeBeforeMove = RandomMoveTimeInterval;
	}

	public override void End(Brain brain)
	{

	}

	public override void Tick(Brain brain)
	{
		brain.TimeBeforeMove -= Time.deltaTime;
		if (brain.TimeBeforeMove <= 0.0f)
		{
			brain.TimeBeforeMove = RandomMoveTimeInterval;

			Vector3 target = GetRandomPositionFromCenter(brain.CenterPosition, IdleMovementsRange);
			Debug.Log("Reach target : " + target + " (current = " + brain.transform.position + ")");
			brain.Movement.ReachPosition(target);
		}
	}

	public override Brain.Action TakeDecision(Brain brain)
	{
		return brain.CurrentAction;
	}

	private Vector3 GetRandomPositionFromCenter(Vector3 center, float range)
	{
		Vector2 randomOffset = Random.insideUnitCircle * range;
		return center + new Vector3(randomOffset.x, 0.0f, randomOffset.y);
	}

	public float RandomMoveTimeInterval => Random.Range(MoveTimeInterval.x, MoveTimeInterval.y);
}
