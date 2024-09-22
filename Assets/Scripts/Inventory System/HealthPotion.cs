using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public override void Use()
    {
        Master.PlayerStats.IncreaseHP(50);
        base.Use();
    }
}
