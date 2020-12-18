using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotsManager : MonoBehaviour
{
	private Dictionary<Vector2Int, CarrotController> _carrots;

	private void Awake()
	{
		_carrots = new Dictionary<Vector2Int, CarrotController>();
	}
}
