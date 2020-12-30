using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopulationCounterUI : MonoBehaviour
{
    [SerializeField] private RabbitsManager _rabbits;
	[SerializeField] private TextMeshProUGUI _text;


	private void Awake()
	{
		_rabbits.onRabbitCountChange += UpdateRabbitCount;
		_text.alpha = 0.0f;
	}

	private void UpdateRabbitCount(object sender, EventArgs data)
	{
		_text.alpha = 1.0f;
		if (sender is RabbitsManager manager)
			_text.text = "Population:\n" + manager.RabbitCount;
	}
}
