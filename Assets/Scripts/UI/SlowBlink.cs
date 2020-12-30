using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlowBlink : MonoBehaviour
{
    public float Speed = 1.0f;
    public TextMeshProUGUI Label;

	private void OnEnable()
	{
        LeanTween.value(gameObject, 0.0f, 1.0f, Speed)
            .setEaseInOutSine()
            .setOnUpdate((float a) =>
            {
                Label.alpha = a;
            })
            .setLoopPingPong();
    }

	private void OnDisable()
	{
		LeanTween.cancel(gameObject);
	}
}
