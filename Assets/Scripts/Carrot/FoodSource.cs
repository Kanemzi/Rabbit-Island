using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
	[Header("References")]
	public CarrotData Data;

	private int _minFoodAmount;
	private int _foodAmountScale;

	public int FoodAmount { get; private set; }

	private void Start()
	{
		_minFoodAmount = Data.RandomMinFoodAmount;
		_foodAmountScale = Data.RandomMaxFoodAmount - _minFoodAmount;

		FoodAmount = _minFoodAmount;
	}

	public void OnGrow(object sender, CarrotController.GrowData data)
	{
		if (data.State == CarrotController.GrowState.Rotten)
			FoodAmount = 0;
		else
			FoodAmount = _minFoodAmount + Mathf.RoundToInt(Data.FoodAmountOverTime.Evaluate(data.GrowPercent) * (float) _foodAmountScale);
	}
}
