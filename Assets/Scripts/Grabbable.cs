using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public class DropData : EventArgs
	{
        public Vector3 GroundPosition;
	}

    public event EventHandler onGrab;
    public event EventHandler<DropData> onDrop;

}
