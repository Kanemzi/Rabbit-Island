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
            //Debug.Log("SpreadChance : " + _spreadChance);
            if (Random.Range(0.0f, 1.0f) < _spreadChance)
			{
                onSpread?.Invoke(this, EventArgs.Empty);
			}
		}
    }

    public void OnGrow(object sender, CarrotController.GrowData data)
    {
     //   Debug.Log(gameObject.name + " -> grow : " + data.GrowPercent + " / " + data.RipePercent);
        if (data.State == CarrotController.GrowState.Rotten 
                || data.State == CarrotController.GrowState.Growing)
            _spreadChance = 0.0f;
        else
            _spreadChance = Data.SpreadChanceOverTime.Evaluate(data.RipePercent);
    }
}
