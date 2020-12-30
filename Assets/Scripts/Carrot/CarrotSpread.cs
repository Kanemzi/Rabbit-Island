using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarrotSpread : MonoBehaviour
{
    public event EventHandler onSpread;

    [Header("References")]
    public CarrotData Data;

    private float _spreadChance;
    private float _timeBeforeSpread;

    private void Start()
    {
        _timeBeforeSpread = Data.TrySpreadInterval;
    }

    private void Update()
    {
        _timeBeforeSpread -= Time.deltaTime;
        if (_timeBeforeSpread <= 0.0f)
		{
            if (Random.Range(0.0f, 1.0f) < _spreadChance)
			{
                onSpread?.Invoke(this, EventArgs.Empty);
			}
            _timeBeforeSpread = Data.TrySpreadInterval;
		}
    }

    public void OnGrow(object sender, CarrotController.GrowData data)
    {
        if (data.State == CarrotController.GrowState.Rotten 
                || data.State == CarrotController.GrowState.Growing)
            _spreadChance = 0.0f;
        else
            _spreadChance = Data.SpreadChanceOverTime.Evaluate(data.RipePercent);
    }
}
