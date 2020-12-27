using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbable : MonoBehaviour
{
    public event EventHandler<GrabData> onGrab;
    public event EventHandler<DropData> onDrop;


    [HideInInspector] public Rigidbody Rigidbody { get; private set; }

    [Header("References")]
    [SerializeField] private Transform _grabHandle;

    public Transform GrabHandle => _grabHandle;

    public class GrabData : EventArgs
    {
        public Vector2Int Cell;
    }

    public class DropData : EventArgs
    {
        public Vector2Int Cell;
        public Vector3 GroundPosition;
    }

    public bool Grabbed { get; private set; }

	private void Awake()
	{
        Grabbed = false;
        Rigidbody = GetComponent<Rigidbody>();        
    }

    public void Grab(Vector2Int cell)
	{
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;

        Grabbed = true;

        onGrab?.Invoke(this, new GrabData
        {
            Cell = cell
        });
	}

    public void Drop(Vector2Int cell, Vector3 groundPosition)
	{
        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;
        transform.position = groundPosition;
        transform.rotation = Quaternion.identity;
        
        Grabbed = false;

        onDrop?.Invoke(this, new DropData {
            Cell = cell,
            GroundPosition = groundPosition
        });
	}
}
