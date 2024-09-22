using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryObject[] InventoryObjects => inventoryObjects;
    public int[] ObjectsCount => objectsCount;

    [Header ("Inventory Capacity")]
    [SerializeField]
    int inventoryCapacity = 5;
    [SerializeField]
    int slotCapacity = 5;

    InventoryObject[] inventoryObjects;
    int[] objectsCount;

    //Actions
    public Action onInventoryUpdated;

    public void Initialize()
    {
        inventoryObjects  = new InventoryObject[inventoryCapacity];
        objectsCount = new int[inventoryCapacity];

        for (int i = 0; i < objectsCount.Length; i++)
        {
            objectsCount[i] = 0;
        }

        Master.HandController.onItemDeleted += RemoveFromList;
    }

    public void AddItem(Item item)
    {
        InventoryObject inventoryObject = item.InventoryObject;

        if (AddToList(inventoryObject)) 
        {
            Destroy(item.gameObject);
            onInventoryUpdated?.Invoke();
        }
    }

    private bool AddToList(InventoryObject item) 
    {
        for (int i = 0; i < inventoryObjects.Length; i++) 
        {
            if (inventoryObjects[i] != null)
            {
                if (inventoryObjects[i].Name == item.Name && objectsCount[i] < slotCapacity)
                {
                    objectsCount[i]++;

                    return true;
                }
            }
            else
            {
                inventoryObjects[i] = item;
                objectsCount[i]++;

                return true;
            }
        }

        return false;
    }

    private void RemoveFromList(int index) 
    {
        if (objectsCount[index] > 1)
        {
            objectsCount[index] -= 1;

        }
        else if (objectsCount[index] == 1)
        {
            objectsCount[index] = 0;
            inventoryObjects[index] = null;
            Master.HandController.Clear();
        }
        else 
        {
            Debug.LogError("List is empty, but you trying to acces it");
        }

        onInventoryUpdated?.Invoke();
    }
}
