using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Item : InteractiveObject, IInteractable, IItem
{
    public PermanentRotation PermanentRotation;
    public InventoryObject InventoryObject;

    public override void GetInfo()
    {

    }

    public override void Interact()
    {
        Debug.Log("Item: Interact");
        Destroy(gameObject);
    }

    public virtual void Use()
    {
        Debug.Log("Item: Used");
        Destroy(gameObject);
    }
}
