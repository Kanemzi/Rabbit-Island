using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RabbitData", menuName = "ScriptableObjects/RabbitData")]
public class RabbitData : ScriptableObject
{
    [Header("Life")]
    [MinMaxSlider(30.0f, 240.0f)]
    public Vector2 DeathAge;
    public float MinGrowScale = 0.4f;
    public float MaxGrowScale = 0.8f;

    [Header("Eyes")]
    public float PerceptionMaxDistance = 4.0f;

    [Header("Movements")]
    public float MinMoveSpeed = 4.0f;
    public float MaxMoveSpeed = 3.0f;
    public AnimationCurve MoveSpeedOverAge;

    [Header("Food")]
    [MinMaxSlider(20, 60)] public Vector2Int MinStomachSize;
    [MinMaxSlider(60, 100)] public Vector2Int MaxStomachSize;
    public AnimationCurve StomachSizeOverAge;
    public AnimationCurve HungryChanceOverStomachFill;
    public float RepleteTreshold = 0.7f;
    public float StomachStepInterval = 1.0f;
    public int StomachEmptyStep = 1;

    [Header("Mate")]
    [Range(10.0f, 60.0f)] public float MinMateAge;
    [Range(20.0f, 240.0f)] public float MaxMateAge;
    public AnimationCurve MateChanceOverTime;
    public float MinStomachFillForMate = 0.5f;
    public float TryMateInterval = 5.0f;

    public float RandomDeathAge => Random.Range(DeathAge.x, DeathAge.y);
    public int RandomMinStomachSize => Random.Range(MinStomachSize.x, MinStomachSize.y);
    public int RandomMaxStomachSize => Random.Range(MaxStomachSize.x, MaxStomachSize.y);
}
