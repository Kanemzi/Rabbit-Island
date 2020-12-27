using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RabbitsManager : MonoBehaviour
{
	[Header("Initialize")]
	public int InitialRabbitsRadius = 3;
	public int InitialRabbitsCount = 3;
	public float InitialMaxAge = 10f;

	[Header("References")]
	[SerializeField] private Grid _grid;
	public GameObject RabbitPrefab;

	private List<RabbitController> _aliveRabbits;

	private void Awake()
	{
		_aliveRabbits = new List<RabbitController>();
	}

	private void Start()
	{
		Init(InitialRabbitsCount, InitialRabbitsRadius);
	}

	/*
	 * Spawns a rabbit on a cell position
	 * Returns the new rabbit or null if the cell is invalid
	 */
	private RabbitController SpawnRabbit(Vector2Int cell)
	{
		if (!_grid.IsValidCell(cell)) return null;
		Vector3 position = _grid.GetRandomPosition(cell);
		GameObject obj = Instantiate(RabbitPrefab, position, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), transform.up));

		RabbitController rabbit = obj.GetComponent<RabbitController>();
		_aliveRabbits.Add(rabbit);

		rabbit.onDead += OnRabbitDeath;
		
		return rabbit;
	}

	private void OnRabbitDeath(object sender, EventArgs data)
	{
		if (sender is RabbitController rabbit)
			RemoveRabbit(rabbit);
	}

	/**
	 * Removes the rabbit from the manager
	 */
	public void RemoveRabbit(RabbitController rabbit)
	{
		_aliveRabbits.Remove(rabbit);
	}

	/**
	 * Removes all the rabbits from the island
	 */
	public void Clear()
	{
		_aliveRabbits.Clear();
	}

	/**
	 * Initializes the island by instancing a certain amount of 
	 * rabbits in the center of the island
	 */
	public void Init(int count, int radius)
	{
		List<Vector2Int> cells = _grid.GetCellsInRadius(radius);

		for (int i = 0; i < count && cells.Count > 0; i++)
		{
			int index = Random.Range(0, cells.Count);
			Vector2Int cell = cells[index];

			RabbitController rabbit = SpawnRabbit(cell);
			rabbit.SetAge(Random.Range(0.0f, InitialMaxAge));
			cells.Remove(cell);
		}
	}
}
