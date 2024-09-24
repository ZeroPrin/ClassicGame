using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummerOfStrength : Item
{
    public override void Use()
    {
        _playerStats.ApplyStrengthBoost(5, 5);
        base.Use();
    }
}
