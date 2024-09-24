using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Item : InteractiveObject, IInteractable, IItem
{
    public PermanentRotation PermanentRotation;
    public InventoryObject InventoryObject;

    protected PlayerStats _playerStats;

    [Inject]
    public void Construct(PlayerStats playerStats) 
    {
        _playerStats = playerStats;
    }

    public override void GetInfo()
    {

    }

    public override void Interact()
    {
        Debug.Log("Interact");
    }

    public virtual void Use()
    {
        Debug.Log("Used");
        Destroy(gameObject);
    }
}
