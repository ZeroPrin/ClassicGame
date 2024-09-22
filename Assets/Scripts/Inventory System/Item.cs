using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractiveObject, IInteractable, IItem
{
    public InventoryObject InventoryObject;
    public override void GetInfo()
    {
        Debug.Log("Item");
    }

    public override void Interact()
    {
        Debug.Log("Interact");
    }

    public void Use()
    {
        Debug.Log("Use");
        Destroy(gameObject);
    }
}
