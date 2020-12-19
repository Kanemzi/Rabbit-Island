using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public event EventHandler onGrab;
    public event EventHandler<DropData> onDrop;

    public class DropData : EventArgs
    {
        public Vector3 GroundPosition;
    }

    public bool Grabbed { get; private set; }

	private void Awake()
	{
        Grabbed = false;
	}
}
