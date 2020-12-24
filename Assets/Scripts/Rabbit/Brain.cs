using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : SerializedMonoBehaviour
{
    [Header("References")]
    public RabbitData Data;
    public Movement Movement;

    public enum Action
	{
        Idle, 
        SearchFood, MoveToFood, Eat,
        WaitMate, JoinMate, Mate
	}

    [SerializeField] private Dictionary<Action, BrainState> _states;

    public Action CurrentAction { get; private set; }
    private BrainState _currentState;

    /* * * * * Internal state * * * * */
    public bool Hungry { get; private set; }

    /* * * * * Work memory * * * * */
    // Idle State
    [HideInInspector] public Vector3 CenterPosition;
    [HideInInspector] public float TimeBeforeMove;

	private void Awake()
	{
	    Hungry = false;
	}
    
    void Start()
    {
        ChangeState(Action.Idle);
    }

    void Update()
    {
        Action decision = _currentState.TakeDecision(this);
        if (decision != CurrentAction)
            ChangeState(decision);

        _currentState?.Tick(this);
    }

    public void ChangeState(Action newAction)
	{
        _currentState?.End(this);

        CurrentAction = newAction;
        _currentState = _states[newAction];

        _currentState?.Begin(this);
	}

    #region [Components callbacks]

    public void OnRabbitHungry(object sender, EventArgs data) => Hungry = true;
    public void OnRabbitReplete(object sender, EventArgs data) => Hungry = false;

	#endregion

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.yellow;
		
        if (_currentState is IdleState idleState)
		{
            Gizmos.DrawWireSphere(CenterPosition, idleState.IdleMovementsRange);
		}
	}
}
