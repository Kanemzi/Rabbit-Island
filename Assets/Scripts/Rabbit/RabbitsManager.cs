using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitsManager : MonoBehaviour
{
    private List<RabbitController> _aliveRabbits;

	private void Awake()
	{
		_aliveRabbits = new List<RabbitController>();
	}
}
