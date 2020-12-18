using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RabbitData", menuName = "ScriptableObjects/RabbitData")]
public class RabbitData : ScriptableObject
{
    [MinMaxSlider(30.0f, 120.0f)]
    public Vector2 DeathAge;
}
