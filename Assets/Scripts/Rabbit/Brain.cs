using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : SerializedMonoBehaviour
{
    public event EventHandler onChangeAction;

    [Header("References")]
    public RabbitData Data;
    public Movement Movement;
    public Eyes Eyes;
    public Stomach Stomach;
    public Reproduction Reproduction;

    public enum Action
	{
        Idle, 
        SearchFood, ReachFood, Eat,
        SearchMate, WaitMate, JoinMate, Mate
	}

    [SerializeField] private Dictionary<Action, BrainState> _states;

    public Action CurrentAction { get; private set; }
    private BrainState _currentState;

    /* * * * * Internal state * * * * */
    public bool Hungry { get; private set; }
    [HideInInspector] public bool WantToMate;
    
    /* * * * * Work memory * * * * */
    // Idle State
    [HideInInspector] public Vector3 CenterPosition;
    [HideInInspector] public float TimeBeforeMove;

    // Food State loop
    [HideInInspector] public CarrotController TargetFood;
    [HideInInspector] public float EatTime;

    // Mate State loop
    [HideInInspector] public RabbitController TargetMate;
    [HideInInspector] public bool MateReached;
    [HideInInspector] public float MateTime;
    [HideInInspector] public bool DoGiveBirth; // true if this rabbit should give birth when mate

    // Reach states 
    [HideInInspector] public bool TargetCancelled;
    [HideInInspector] public bool TargetReached;
    [HideInInspector] public bool FoodReached;
    [HideInInspector] public bool HasCheckedArea;


    private void Awake()
	{
	    Hungry = false;
        WantToMate = false;
        TargetFood = null;
        TargetMate = null;
	}
    
    void Start()
    {
        ChangeState(Action.Idle);
    }

	private void OnDestroy()
	{
        _currentState?.End(this);
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
        onChangeAction?.Invoke(this, EventArgs.Empty);
	}

    #region [Components callbacks]

    public void OnRabbitHungry(object sender, EventArgs data) => Hungry = true;
    public void OnRabbitReplete(object sender, EventArgs data) => Hungry = false;
    public void OnWantToMate(object sender, EventArgs data) => WantToMate = true;
    public void OnDrop(object sender, Grabbable.DropData data) => ChangeState(Action.Idle);

    #endregion

    private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Data.PerceptionMaxDistance);

        Gizmos.color = Color.yellow;
		
        if (_currentState is MovementState movementState)
		{
            Gizmos.DrawWireSphere(CenterPosition, movementState.MovementsRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(movementState.Target, 0.2f);
		}
	}
}
