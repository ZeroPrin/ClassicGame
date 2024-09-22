using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractiveObject, IInteractable, IItem
{
    public PermanentRotation PermanentRotation;
    public InventoryObject InventoryObject;
    public override void GetInfo()
    {
        //Debug.Log("Item");
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
