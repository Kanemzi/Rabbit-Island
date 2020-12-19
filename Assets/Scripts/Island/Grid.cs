using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
	[OnValueChanged("ClearCache")]
	[OnValueChanged("ComputeOffset")]
	public float CellSize = 0.4f;
	public float RandomPlacementPadding = 0.05f;

	[Header("Debug")]
	public int GridGizmosSize = 20;
	public bool DrawExactPlacements = true;
	public bool DrawRandomPlacements = false;

	private Dictionary<Vector2Int, bool> _validCellsCache;
	private Vector3 _offset;
	private float _maxRandomOffset;

	private void Awake()
	{
		_validCellsCache = new Dictionary<Vector2Int, bool>();
		ComputeOffset();
		_maxRandomOffset = (CellSize / 2.0f) - RandomPlacementPadding;
	}

	/**
	 * Checks if the cell is in the navmesh of the current island
	 */
	public bool IsValidCell(Vector2Int cell)
	{
		if (!_validCellsCache.ContainsKey(cell))
		{
			Vector3 position = GetPosition(cell);
			bool valid = false;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(position, out hit, CellSize / 2.0f, NavMesh.AllAreas))
			{
				valid = true;
			}
			_validCellsCache.Add(cell, valid);
			return valid;
		}
		else return _validCellsCache[cell];
	}

	public Vector2Int GetCell(Vector3 position)
	{
		Vector3 relativePosition = position - transform.position;
		int x = Mathf.FloorToInt(relativePosition.x / CellSize);
		int y = Mathf.FloorToInt(relativePosition.z / CellSize);
		return new Vector2Int(x, y);
	}

	public Vector3 GetPosition(Vector2Int cell)
	{
		return transform.position + new Vector3(cell.x, 0.0f, cell.y) * CellSize + _offset;
	}

	/**
	 * Returns a random 3D position inside a cell of the grid
	 */
	public Vector3 GetRandomPosition(Vector2Int cell)
	{
		Vector3 position = GetPosition(cell);
		position.x += Random.Range(-_maxRandomOffset, _maxRandomOffset);
		position.z += Random.Range(-_maxRandomOffset, _maxRandomOffset);
		return position;
	}

	/**
	 * Returns a list of the valid cells in a certain range from the center
	 * of the grid
	 */
	public List<Vector2Int> GetCellsInRadius(int radius)
	{
		List<Vector2Int> tilesInRange = new List<Vector2Int>();

		for (int y = -radius; y <= radius; y++)
		{
			for (int x = -radius; x <= radius; x++)
			{
				if (y * y + x * x > radius * radius) continue;

				Vector2Int cell = new Vector2Int(x, y);
				if (IsValidCell(cell))
					tilesInRange.Add(cell);
			}
		}

		return tilesInRange;
	}


	public void ClearCache()
	{
		_validCellsCache.Clear();
	}

	private void ComputeOffset()
	{
		_offset = new Vector3(CellSize / 2.0f, 0.0f, CellSize / 2.0f);
	}

	private void OnDrawGizmosSelected()
	{
		if (_validCellsCache == null) return;
		if (!Application.isPlaying)
			Random.InitState(1);

		for(int y = -GridGizmosSize / 2; y < GridGizmosSize / 2; y++)
		{
			for (int x = -GridGizmosSize / 2; x < GridGizmosSize / 2; x++)
			{
				Vector2Int cell = new Vector2Int(x, y);
				if (!IsValidCell(cell)) continue;

				Vector3 position = GetPosition(cell);
				if (DrawExactPlacements)
				{
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(position, 0.05f);
				}

				if (DrawRandomPlacements && !Application.isPlaying) {
					Gizmos.color = Color.red;
					Vector3 randomPosition = GetRandomPosition(cell);
					Gizmos.DrawSphere(randomPosition, 0.07f);
				
					if (DrawExactPlacements) Gizmos.DrawLine(position, randomPosition);
				}
			}
		}
	}
}
