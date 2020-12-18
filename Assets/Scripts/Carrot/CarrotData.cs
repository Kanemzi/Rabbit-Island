using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarrotData", menuName = "ScriptableObjects/CarrotData")]
public class CarrotData : ScriptableObject
{
    [Header("Growing")]
    [MinMaxSlider(10.0f, 30.0f)] public Vector2 GrowTime;
    [MinMaxSlider(10.0f, 40.0f)] public Vector2 RotTime;
    public float MinGrowScale = 0.4f;
    public float MaxGrowScale = 1.2f;

    [Header("Food")]
    [MinMaxSlider(1, 20)] public Vector2Int MinFoodAmount;
    [MinMaxSlider(1, 200)] public Vector2Int MaxFoodAmount;
    public AnimationCurve FoodAmountOverTime;

    [Header("Spread")]
    public AnimationCurve SpreadChanceOverTime;
    public float TrySpreadInterval = 1.0f;
}
