using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public event EventHandler<GrabData> onTargetCellChange;
    public event EventHandler<GrabData> onTargetRabbitChange;
    public event EventHandler<GrabData> onTryGrab;
    public event EventHandler<GrabData> onDrop;

    public class GrabData : EventArgs
    {
        public Vector2Int Cell;
        public Vector3 Position;
        public bool Valid;
        public RabbitController Rabbit;
    }

    [Header("Parameters")]
    public float GroundHeight = 0.0f;
    public LayerMask RabbitsLayer;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Grid _grid;
    [SerializeField] private GrabHandler _grabHandler;

    [SerializeField] private CellHighlight _cellHighlight;

    private Plane _groundPlane;
    private Ray _mouseRay;

    public Vector2Int TargetCell { get; private set; }
    public RabbitController TargetRabbit { get; private set; }
    public bool TargetValid { get; private set; }

	void Awake()
    {
        _groundPlane = new Plane(Vector3.up, GroundHeight);

        _cellHighlight.Grid = _grid;
    }

	void Start()
	{
        onTargetCellChange += _cellHighlight.UpdateTarget;
        onTryGrab += _grabHandler.Grab;
        onDrop += _grabHandler.Drop;
    }

	void Update()
    {
        _mouseRay = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

        MoveToMousePosition();
        UpdateTargetCell();
        UpdateTargetRabbit();
    }

    private void MoveToMousePosition()
	{
        if (_groundPlane.Raycast(_mouseRay, out float distance))
		{
            transform.position = _mouseRay.GetPoint(distance);
		}
    }

    private void UpdateTargetCell()
	{
        Vector2Int newCell = _grid.GetCell(transform.position);
        if (TargetCell.Equals(newCell)) return;

        TargetCell = newCell;
        TargetValid = _grid.IsValidCell(TargetCell);
        onTargetCellChange?.Invoke(this, new GrabData
        {
            Cell = TargetCell,
            Position = _grid.GetPosition(TargetCell),
            Valid = TargetValid
        });
    }

    private void UpdateTargetRabbit()
	{
        RabbitController newTarget = null;
        RaycastHit hit;
        if (Physics.Raycast(_mouseRay, out hit, 100, RabbitsLayer))
		{
            newTarget = hit.collider.GetComponent<RabbitController>();
		}

        if (newTarget != TargetRabbit)
		{
            TargetRabbit = newTarget;

            onTargetRabbitChange?.Invoke(this, new GrabData
            {
                Cell = TargetCell,
                Position = _grid.GetPosition(TargetCell),
                Valid = TargetValid,
                Rabbit = TargetRabbit
            });
        }
    }


    public void OnGrabAction(InputAction.CallbackContext ctx)
	{
        GrabData data = new GrabData
        {
            Cell = TargetCell,
            Position = _grid.GetPosition(TargetCell),
            Valid = TargetValid,
            Rabbit = TargetRabbit
        };

        if (ctx.performed)
            onTryGrab?.Invoke(this, data);
        else if (ctx.canceled)
            onDrop?.Invoke(this, data);
    }
}
