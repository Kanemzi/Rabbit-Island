using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
	public event EventHandler onEaten;

	[Header("References")]
	public CarrotData Data;

	private int _minFoodAmount;
	private int _foodAmountScale;
	private int _foodBonus;

	public int FoodAmount { get; private set; }

	private void Start()
	{
		_foodBonus = 0;
		_minFoodAmount = Data.RandomMinFoodAmount;
		_foodAmountScale = Data.RandomMaxFoodAmount - _minFoodAmount;

		FoodAmount = _minFoodAmount;
	}

	public void OnGrow(object sender, CarrotController.GrowData data)
	{
		if (data.State == CarrotController.GrowState.Rotten)
			FoodAmount = 0;
		else
			FoodAmount = _foodBonus + _minFoodAmount + Mathf.RoundToInt(Data.FoodAmountOverTime.Evaluate(data.GrowPercent) * (float) _foodAmountScale);
	}

	public void Eat()
	{
		onEaten?.Invoke(this, EventArgs.Empty);
		Destroy(gameObject);
	}

	public void Merge(FoodSource food)
	{
		_foodBonus += food.FoodAmount;
	}
}
