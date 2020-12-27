using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotAnimations : MonoBehaviour
{
	[Header("References")]
	public CarrotData Data;

	[SerializeField] private GameObject _body;

	private float _scaling;

	private void Start()
	{
		_scaling = Data.MaxGrowScale - Data.MinGrowScale;
		UpdateScale(Data.MinFoodAmount.x);
	}

	public void OnRot(object sender, EventArgs data)
	{
		LeanTween.rotateZ(_body, 60.0f, 1.5f).setOnComplete(() =>
		{
			Destroy(gameObject);
		});
	}

	public void OnGrow(object sender, CarrotController.GrowData data)
	{
		if (sender is CarrotController carrot)
		UpdateScale(carrot.FoodSource.FoodAmount);
	}

	public void UpdateScale(int foodAmount)
	{
		float foodPercent = (float)foodAmount / Data.MaxFoodAmount.y;
		float newScale = Data.MinGrowScale + _scaling * foodPercent;
		newScale = Mathf.Min(newScale, Data.MaxAbsoluteScale);
		transform.localScale = Vector3.one * newScale;
	}
}
