using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private CarrotsManager _carrots;
	[SerializeField] private Joint _grabHandle;

	private Grabbable _grabbed;

	private void Awake()
	{
		_grabbed = null;
	}

	public void Grab(object sender, HandController.TargetCellData data)
	{
		if (_grabbed) return;
		if (!data.Valid) return;

		// TODO : Later, take in account rabbit targets

		CarrotController carrot = _carrots.GetCarrotAt(data.Cell);
		if (!carrot) return;

		Grabbable grabbed = carrot.GetComponent<Grabbable>();
		if (!grabbed) return;

		grabbed.Grab(data.Cell);

		_grabbed = grabbed;

		_grabHandle.connectedBody = grabbed.Rigidbody;
		_grabHandle.connectedAnchor = grabbed.GrabHandle.position;
	}

	public void Drop(object sender, HandController.TargetCellData data)
	{
		if (!_grabbed) return;

		_grabbed.Drop(data.Cell, data.Position);
		_grabbed = null;

		_grabHandle.connectedBody = null;
	}
}
