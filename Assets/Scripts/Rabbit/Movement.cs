using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : MonoBehaviour
{
	public event EventHandler onStartMoving;
	public event EventHandler onCancelMove;
	public event EventHandler onTargetReached;

	[Header("References")]
    public RabbitData Data;

	public Vector3 Target { get; protected set; }

	[SerializeField] private NavMeshAgent _agent;
	private NavMeshPath _path;
	private float _currentSpeed;
	private bool _currentTargetReached;

	private void Start()
	{
		_currentSpeed = Data.MinMoveSpeed;
		_path = new NavMeshPath();
	}

	public bool CanReachPosition(Vector3 position)
	{
		NavMeshPath path = new NavMeshPath();
		_agent.CalculatePath(position, path);

		return path.status == NavMeshPathStatus.PathComplete;
	}

	public bool ReachPosition(Vector3 position)
	{
		_agent.speed = _currentSpeed;

		if (!PositionReached())
		{
			onCancelMove?.Invoke(this, EventArgs.Empty);
		}

		Target = position;
		_currentTargetReached = false;

		onStartMoving?.Invoke(this, EventArgs.Empty);
		return _agent.SetDestination(Target);
	}

	public bool PositionReached()
	{
		return _agent.remainingDistance <= _agent.stoppingDistance;
	}

	private void Update()
	{
		if (_agent.isActiveAndEnabled && !_currentTargetReached)
		{
			_currentTargetReached = PositionReached();
			onTargetReached?.Invoke(this, EventArgs.Empty);
		}
	}

	private void UpdateSpeed(float growPercent)
	{
		float speedScale = Data.MaxMoveSpeed - Data.MinMoveSpeed;
		_currentSpeed = Data.MinMoveSpeed + Data.MoveSpeedOverAge.Evaluate(growPercent) * speedScale;
	}

	public void OnRabbitGrow(object sender, EventArgs data)
	{
		if (sender is RabbitController rabbit)
			UpdateSpeed(rabbit.LifePercent);
	}
}
