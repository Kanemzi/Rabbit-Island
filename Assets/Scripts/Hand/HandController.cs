using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour
{
    public event EventHandler<TargetCellData> onTargetCellChange;
    public event EventHandler<TargetCellData> onTryGrab;
    public event EventHandler<TargetCellData> onDrop;

    public class TargetCellData : EventArgs
    {
        public Vector2Int Cell;
        public Vector3 Position;
        public bool Valid;
    }

    [Header("Parameters")]
    public float GroundHeight = 0.0f;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Grid _grid;
    [SerializeField] private GrabHandler _grabHandler;

    [SerializeField] private CellHighlight _cellHighlight;

    private Plane _groundPlane;

    public Vector2Int TargetCell { get; private set; }
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
        MoveToMousePosition();
        UpdateTargetCell();
    }

    private void MoveToMousePosition()
	{
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
	    if (_groundPlane.Raycast(ray, out float distance))
		{
            transform.position = ray.GetPoint(distance);
		}
    }

    private void UpdateTargetCell()
	{
        Vector2Int newCell = _grid.GetCell(transform.position);
        if (TargetCell.Equals(newCell)) return;

        TargetCell = newCell;
        TargetValid = _grid.IsValidCell(TargetCell);
        onTargetCellChange?.Invoke(this, new TargetCellData
        {
            Cell = TargetCell,
            Position = _grid.GetPosition(TargetCell),
            Valid = TargetValid
        });
    }


    public void OnGrabAction(InputAction.CallbackContext ctx)
	{
        TargetCellData data = new TargetCellData
        {
            Cell = TargetCell,
            Position = _grid.GetPosition(TargetCell),
            Valid = TargetValid
        };

        if (ctx.performed)
            onTryGrab?.Invoke(this, data);
        else if (ctx.canceled)
            onDrop?.Invoke(this, data);
    }
}
