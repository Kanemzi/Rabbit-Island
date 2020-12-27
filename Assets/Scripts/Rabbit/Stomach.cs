using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Stomach : MonoBehaviour
{
    public event EventHandler onHungry;
	public event EventHandler onReplete;
	public event EventHandler onStarve;
	public event EventHandler onFoodPointsChange;
	public event EventHandler onGrow;

    [Header("References")]
    public RabbitData Data;


	private int _minStomachSize;
	private int _maxStomachSize;

	private float _maxFoodPoints;
	private float _foodPoints;

	private float _nextStomachStepTime;

	public float FillPercent => _foodPoints / _maxFoodPoints;
	public int MaxFoodPoints => (int) _maxFoodPoints;
	public int FoodPoints => (int) _foodPoints;
    public bool IsHungry { get; private set; }

	private void Start()
	{
		_minStomachSize = Data.RandomMinStomachSize;
		_maxStomachSize = Data.RandomMaxStomachSize;
		
		_maxFoodPoints = _minStomachSize;
		_foodPoints = _maxFoodPoints;

		_nextStomachStepTime = Data.StomachStepInterval;
	}

	private void Update()
	{
		_nextStomachStepTime -= Time.deltaTime;
		if (_nextStomachStepTime <= 0.0f)
		{
			_nextStomachStepTime = Data.StomachStepInterval;

			DoStep();
		}
	}

	private void DoStep()
	{
		if (_foodPoints <= 0)
		{
			onStarve?.Invoke(this, EventArgs.Empty);
			return;
		}

		_foodPoints -= Data.StomachEmptyStep;
		onFoodPointsChange?.Invoke(this, EventArgs.Empty);

		float realizeHungryChance = Random.Range(0.0f, 1.0f);
		if (realizeHungryChance <= Data.HungryChanceOverStomachFill.Evaluate(FillPercent))
		{
			//			Debug.Log("I'm hungry " + FillPercent);

			IsHungry = true;
			onHungry?.Invoke(this, EventArgs.Empty);
		}

//		Debug.Log("Stomach : " + _foodPoints + "/" + _maxFoodPoints + " -> " + FillPercent + "%");
	}

	private void ChangeStomachSize(float growPercent)
	{
		float stomachScale = _maxStomachSize - _minStomachSize;
		_maxFoodPoints = _minStomachSize + Data.StomachSizeOverAge.Evaluate(growPercent) * stomachScale;
		onGrow?.Invoke(this, EventArgs.Empty);
	}

	public void OnRabbitGrow(object sender, EventArgs data)
	{
		if (sender is RabbitController rabbit)
			ChangeStomachSize(rabbit.LifePercent);
	}

	public void Eat(FoodSource food)
	{
		_foodPoints += food.FoodAmount;
		if (_foodPoints > _maxFoodPoints) _foodPoints = Mathf.FloorToInt(_maxFoodPoints);

		food.Eat();
		onFoodPointsChange?.Invoke(this, EventArgs.Empty);

		if (FillPercent >= Data.RepleteTreshold)
		{
			IsHungry = false;
			onReplete?.Invoke(this, EventArgs.Empty);
		}
	}
}
