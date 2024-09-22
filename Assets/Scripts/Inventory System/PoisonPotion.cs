using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPotion : Item
{
    public override void Use()
    {
        Master.PlayerStats.ApplyPoison(5, 5);
        base.Use();
    }
}
