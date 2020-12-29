using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class MovementState : BrainState
{
    public event EventHandler onNewTarget;

    public float MovementsRange = 6.0f;
    [MinMaxSlider(0.1f, 16.0f)] public Vector2 MoveTimeInterval;

    public Vector3 Target { get; protected set; }

    public override void Begin(Brain brain)
    {
        brain.CenterPosition = brain.transform.position;
        brain.TimeBeforeMove = RandomMoveTimeInterval / 4.0f;
    }

    public override void Tick(Brain brain)
    {
        brain.TimeBeforeMove -= Time.deltaTime;
        if (brain.TimeBeforeMove <= 0.0f)
        {
            brain.TimeBeforeMove = RandomMoveTimeInterval;

            Target = GetRandomPositionFromCenter(brain.CenterPosition, MovementsRange);
            brain.Movement.ReachPosition(Target);
            onNewTarget?.Invoke(brain.Movement, EventArgs.Empty);
        }
    }

    private Vector3 GetRandomPositionFromCenter(Vector3 center, float range)
    {
        Vector2 randomOffset = Random.insideUnitCircle * range;
        return center + new Vector3(randomOffset.x, 0.0f, randomOffset.y);
    }

    public float RandomMoveTimeInterval => Random.Range(MoveTimeInterval.x, MoveTimeInterval.y);
}
