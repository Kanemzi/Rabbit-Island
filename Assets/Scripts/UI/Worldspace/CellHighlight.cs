using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellHighlight : MonoBehaviour
{
	private const string MATERIAL_VALID_PARAM = "Vector1_ebfc0b2722c14409a1e68f69aff38d52";

	[Header("Parameters")]
	public float DistanceFromGround = 0.01f;

	[Header("References")]
	[SerializeField] private GameObject _body;

	[HideInInspector] public Grid Grid;
	private Material _highlightMaterial;
	private bool _valid;

	public bool Valid
	{
		get => _valid;
		set
		{
			if (value == _valid) return;
			
			_valid = value;
			_highlightMaterial.SetFloat(MATERIAL_VALID_PARAM, _valid ? 1.0f : 0.0f);
		}
	}

	private void Awake()
	{
		Material copy = new Material(_body.GetComponent<MeshRenderer>().sharedMaterial);
		
		_highlightMaterial = copy;
		_body.GetComponent<MeshRenderer>().sharedMaterial = _highlightMaterial;

		// Unbundle the element from it's prefab
		transform.SetParent(null);
	}

	private void Start()
	{
		transform.localScale = Grid.CellSize * Vector3.one;
	}

	public void UpdateTarget(object sender, HandController.TargetCellData data)
	{
		Valid = data.Valid;
		LeanTween.move(gameObject, data.Position , 0.05f);
	}
}
