using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfWisdom : Item
{
    public override void Use()
    {
        Master.PlayerStats.IncreaseWisdom(1);
        base.Use();
    }
}
