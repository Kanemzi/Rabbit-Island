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
	}

	private void UpdateRabbitCount(object sender, EventArgs data)
	{
		if (sender is RabbitsManager manager)
			_text.text = "Population:\n" + manager.RabbitCount;
	}
}
