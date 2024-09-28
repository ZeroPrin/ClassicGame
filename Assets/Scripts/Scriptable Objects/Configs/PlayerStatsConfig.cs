using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsConfig", menuName = "Configs/PlayerStatsConfig")]
public class PlayerStatsConfig : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: SerializeField] public float JumpForce { get; private set; } = 5f;
}


