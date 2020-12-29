using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Reproduction : MonoBehaviour
{
    public event EventHandler<GiveBirthData> onGiveBirth;
	public event EventHandler onWantToMate;

    public class GiveBirthData : EventArgs
	{
        public int Count;
	}

    [Header("References")]
    public RabbitData Data;
	public RabbitController Rabbit;

    public int ChildrenLeft {get; private set; }

	private float _nextMateStepTime;
	private float _lifePercent;

	private void Awake()
	{
        ChildrenLeft = Data.MaxChildrenCount;
		_lifePercent = 0.0f;
		_nextMateStepTime = Data.TryMateInterval;
	}

	private void Update()
	{
		if (!Rabbit.ReadyToMate) return;
		if (Rabbit.Brain.WantToMate) return; // Don't process if already wants to mate

		_nextMateStepTime -= Time.deltaTime;
		if (_nextMateStepTime <= 0.0f)
		{
			_nextMateStepTime = Data.TryMateInterval;

			DoStep();
		}
	}

	private void DoStep()
	{
		float wantToMateChance = Random.Range(0.0f, 1.0f);
		if (wantToMateChance <= Data.MateChanceOverTime.Evaluate(_lifePercent))
		{
			onWantToMate?.Invoke(this, EventArgs.Empty);
		}
	}

	public void OnRabbitGrow(object sender, EventArgs data)
	{
		if (sender is RabbitController rabbit)
			_lifePercent = rabbit.LifePercent;
	}

	public void GiveBirth(Reproduction other, int count)
	{
        ChildrenLeft -= count;
        other.ChildrenLeft -= count;

        onGiveBirth?.Invoke(this, new GiveBirthData
        {
            Count = count
        });
	}
}
