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

    [Header("Inventory Data")]
    [SerializeField]
    private InventoryData savedInventoryData;

    InventoryObject[] inventoryObjects;
    int[] objectsCount;
    InventoryObject currentObject = null;
    int currentIndex = -1;



    [Header ("Actions")]
    public Action onInventoryUpdated;

    public void Initialize()
    {
        inventoryObjects  = new InventoryObject[inventoryCapacity];
        objectsCount = new int[inventoryCapacity];

        for (int i = 0; i < objectsCount.Length; i++)
        {
            objectsCount[i] = 0;
        }

        currentObject = null;
        currentIndex = -1;

        Master.HandController.onItemDeleted += RemoveFromList;

        LoadInventory();
    }

    public void Deinitialize() 
    {
        SaveInventory();
    }

    #region Add/Remove Item
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
        }

        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            if (inventoryObjects[i] == null)
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
    #endregion

    #region Switch Items

    public void SwitchNext() 
    {
        for (int i = currentIndex + 1; i < inventoryObjects.Length; i++)
        {
            if (inventoryObjects[i] != null)
            {
                currentIndex = i;
                currentObject = inventoryObjects[i];

                Master.HandController.SetObject(inventoryObjects[i].Prefab, i);

                return;
            }
        }

        SafeSearch();
    }

    public void SafeSearch()
    {
        for (int i = 0; i < inventoryObjects.Length; i++)
        {
            if (inventoryObjects[i] != null)
            {
                currentIndex = i;
                currentObject = inventoryObjects[i];

                Master.HandController.SetObject(inventoryObjects[i].Prefab, i);

                return;
            }
        }

        currentIndex = -1;
        currentObject = null;
    }

    public void SwitchPrevious() 
    {
        for (int i = currentIndex - 1; i >= 0; i--)
        {
            if (inventoryObjects[i] != null)
            {
                currentIndex = i;
                currentObject = inventoryObjects[i];

                Master.HandController.SetObject(inventoryObjects[i].Prefab, i);

                return;
            }
        }

        SafeSearchInvert();
    }

    public void SafeSearchInvert()
    {
        for (int i = inventoryObjects.Length - 1; i >= 0; i--)
        {
            if (inventoryObjects[i] != null)
            {
                currentIndex = i;
                currentObject = inventoryObjects[i];

                Master.HandController.SetObject(inventoryObjects[i].Prefab, i);

                return;
            }
        }

        currentIndex = -1;
        currentObject = null;
    }

    #endregion

    #region Save/Load
    public void SaveInventory()
    {
        if (savedInventoryData != null)
        {
            savedInventoryData.inventoryObjects.Clear();
            savedInventoryData.objectsCount.Clear();

            for (int i = 0; i < inventoryObjects.Length; i++)
            {
                if (inventoryObjects[i] != null)
                {
                    savedInventoryData.inventoryObjects.Add(inventoryObjects[i]);
                    savedInventoryData.objectsCount.Add(objectsCount[i]);
                }
            }

            Debug.Log("Inventory saved successfully.");
        }
    }

    public void LoadInventory()
    {
        if (savedInventoryData != null)
        {
            for (int i = 0; i < savedInventoryData.inventoryObjects.Count; i++)
            {
                inventoryObjects[i] = savedInventoryData.inventoryObjects[i];
                objectsCount[i] = savedInventoryData.objectsCount[i];
            }

            onInventoryUpdated?.Invoke();
            Debug.Log("Inventory loaded successfully.");
        }
    }
    #endregion
}
