using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public event EventHandler<TargetCellChangeData> onTargetCellChange;

    public class TargetCellChangeData : EventArgs
    {
        public Vector2Int Cell;
        public Vector3 Position;
        public bool Valid;
    }

    [Header("Parameters")]
    public float GroundHeight = 0.0f;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Joint _grabHandle;
    [SerializeField] private Grid _grid;

    [SerializeField] private CellHighlight _cellHighlight;

    private Inputs inputs;
    private Plane _groundPlane;

    private Vector2Int _targetCell;
    private bool _targetValid;

	void Awake()
    {
        inputs = new Inputs();
        _groundPlane = new Plane(Vector3.up, GroundHeight);

        _cellHighlight.Grid = _grid;
    }

	void Start()
	{
        inputs.Hand.Grab.performed += Grab;
        inputs.Hand.Grab.canceled += Drop;

        onTargetCellChange += _cellHighlight.UpdateTarget;
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
        if (_targetCell.Equals(newCell)) return;

        _targetCell = newCell;
        _targetValid = _grid.IsValidCell(_targetCell);
        onTargetCellChange?.Invoke(this, new TargetCellChangeData
        {
            Cell = _targetCell,
            Position = _grid.GetPosition(_targetCell),
            Valid = _targetValid
        });
    }

    public void Grab(InputAction.CallbackContext ctx)
	{

	}

    public void Drop(InputAction.CallbackContext ctx)
    {

    }
}
