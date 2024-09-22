using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Item
{
    public override void Use()
    {
        Master.PlayerStats.ApplySpeedBoost(5, 5);
        base.Use();
    }
}
