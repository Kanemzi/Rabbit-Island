using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarrotData", menuName = "ScriptableObjects/CarrotData")]
public class CarrotData : ScriptableObject
{
    [Header("Growing")]
    [MinMaxSlider(4.0f, 30.0f)] public Vector2 GrowTime;
    [MinMaxSlider(10.0f, 40.0f)] public Vector2 RotTime;
    public float MinGrowScale = 0.4f;
    public float MaxGrowScale = 1.2f;
    public float MaxAbsoluteScale = 1.3f;

    [Header("Food")]
    [MinMaxSlider(1, 20)] public Vector2Int MinFoodAmount;
    [MinMaxSlider(1, 200)] public Vector2Int MaxFoodAmount;
    public AnimationCurve FoodAmountOverTime;
    public int MaxFoodBonus = 30;

    [Header("Spread")]
    public AnimationCurve SpreadChanceOverTime;
    public float TrySpreadInterval = 1.0f;

    public float RandomGrowTime => Random.Range(GrowTime.x, GrowTime.y);
    public float RandomRotTime => Random.Range(RotTime.x, RotTime.y);
    public int RandomMinFoodAmount => Random.Range(MinFoodAmount.x, MinFoodAmount.y);
    public int RandomMaxFoodAmount => Random.Range(MaxFoodAmount.x, MaxFoodAmount.y);
}
