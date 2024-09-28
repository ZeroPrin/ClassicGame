using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField] public float RotationSpeed { get; private set; } = 200f;
    [field: SerializeField] public float MaxPitch { get; private set; } = 80f;
    [field: SerializeField] public float GroundCheckDistance { get; private set; } = 1f;
    [field: SerializeField] public float MinDistanceForJump { get; private set; } = 0.1f;
    [field: SerializeField] public float SmoothTime { get; private set; } = 0.1f;
    [field: SerializeField] public float RotationSmoothTime { get; private set; } = 0.05f;
    [field: SerializeField] public float InteractionDistance { get; private set; } = 3f;
}