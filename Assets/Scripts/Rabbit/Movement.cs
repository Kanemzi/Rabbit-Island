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

    [SerializeField] private NavMeshAgent _agent;
	private NavMeshPath _path;
	private float _currentSpeed;

	private void Start()
	{
		_currentSpeed = Data.MinMoveSpeed;
		_path = new NavMeshPath();
	}

	public bool CanReachPosition(Vector3 position)
	{
		NavMeshPath path = new NavMeshPath();
		return _agent.CalculatePath(position, path);
	}

	public bool ReachPosition(Vector3 position)
	{
		return _agent.SetDestination(position);
	}

	private void Update()
	{
		
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
