using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookOfWisdom : Item
{
    public override void Use()
    {
        _playerStats.IncreaseWisdom(1);
        base.Use();
    }
}
