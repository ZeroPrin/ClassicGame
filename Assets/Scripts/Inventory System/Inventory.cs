using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Inventory : IInitializable, IDisposable
{
    [Header("Injected Components")]
    private HandController _handController;
    private PlayerController _playerController;

    [Header ("Other")]
    public InventoryObject[] InventoryObjects => _inventoryObjects;
    public int[] ObjectsCount => _objectsCount;

    [Header ("Inventory Capacity")]
    int _inventoryCapacity = 5;
    int _slotCapacity = 5;

    [Header("Inventory Data")]
    private InventoryData _savedInventoryData;

    private InventoryObject[] _inventoryObjects;
    private int[] _objectsCount;
    private InventoryObject _currentObject = null;
    private int _currentIndex = -1;

    [Header ("Actions")]
    public Action OnInventoryUpdated;

    [Inject]
    public void Construct(HandController handController, PlayerController playerController, InventoryData savedInventoryData) 
    {
        _handController = handController;
        _playerController = playerController;
        _savedInventoryData = savedInventoryData;
    }

    void IInitializable.Initialize()
    {
        _playerController.OnTocheInteractiveObject += AddItem;

        _inventoryObjects  = new InventoryObject[_inventoryCapacity];
        _objectsCount = new int[_inventoryCapacity];

        for (int i = 0; i < _objectsCount.Length; i++)
        {
            _objectsCount[i] = 0;
        }

        _currentObject = null;
        _currentIndex = -1;

        _handController.OnItemDeleted += RemoveFromList;

        LoadInventory();
    }

    void IDisposable.Dispose() 
    {
        SaveInventory();
    }

    #region Add/Remove Item
    public void AddItem(InteractiveObject interact)
    {
        Debug.Log("AddItem");

        Item item = interact.GetComponent<Item>();

        if (item == null) 
        {
            Debug.Log(item);
            return;
        }

        InventoryObject inventoryObject = item.InventoryObject;

        if (AddToList(inventoryObject)) 
        {
            //Destroy(item.gameObject);
            OnInventoryUpdated?.Invoke();
        }
    }

    private bool AddToList(InventoryObject item) 
    {
        for (int i = 0; i < _inventoryObjects.Length; i++)
        {
            if (_inventoryObjects[i] != null)
            {
                if (_inventoryObjects[i].Name == item.Name && _objectsCount[i] < _slotCapacity)
                {
                    _objectsCount[i]++;

                    return true;
                }
            }
        }

        for (int i = 0; i < _inventoryObjects.Length; i++)
        {
            if (_inventoryObjects[i] == null)
            {
                _inventoryObjects[i] = item;
                _objectsCount[i]++;

                return true;
            }
        }

        return false;
    }

    private void RemoveFromList(int index) 
    {
        if (_objectsCount[index] > 1)
        {
            _objectsCount[index] -= 1;

        }
        else if (_objectsCount[index] == 1)
        {
            _objectsCount[index] = 0;
            _inventoryObjects[index] = null;
            _handController.Clear();
        }
        else 
        {
            Debug.LogError("List is empty, but you trying to acces it");
        }

        OnInventoryUpdated?.Invoke();
    }
    #endregion

    #region Switch Items

    public void SwitchNext() 
    {
        for (int i = _currentIndex + 1; i < _inventoryObjects.Length; i++)
        {
            if (_inventoryObjects[i] != null)
            {
                _currentIndex = i;
                _currentObject = _inventoryObjects[i];

                _handController.SetObject(_inventoryObjects[i].Prefab, i);

                return;
            }
        }

        SafeSearch();
    }

    public void SafeSearch()
    {
        for (int i = 0; i < _inventoryObjects.Length; i++)
        {
            if (_inventoryObjects[i] != null)
            {
                _currentIndex = i;
                _currentObject = _inventoryObjects[i];

                _handController.SetObject(_inventoryObjects[i].Prefab, i);

                return;
            }
        }

        _currentIndex = -1;
        _currentObject = null;
    }

    public void SwitchPrevious() 
    {
        for (int i = _currentIndex - 1; i >= 0; i--)
        {
            if (_inventoryObjects[i] != null)
            {
                _currentIndex = i;
                _currentObject = _inventoryObjects[i];

                _handController.SetObject(_inventoryObjects[i].Prefab, i);

                return;
            }
        }

        SafeSearchInvert();
    }

    public void SafeSearchInvert()
    {
        for (int i = _inventoryObjects.Length - 1; i >= 0; i--)
        {
            if (_inventoryObjects[i] != null)
            {
                _currentIndex = i;
                _currentObject = _inventoryObjects[i];

                _handController.SetObject(_inventoryObjects[i].Prefab, i);

                return;
            }
        }

        _currentIndex = -1;
        _currentObject = null;
    }

    #endregion

    #region Save/Load
    public void SaveInventory()
    {
        if (_savedInventoryData != null)
        {
            _savedInventoryData.inventoryObjects.Clear();
            _savedInventoryData.objectsCount.Clear();

            for (int i = 0; i < _inventoryObjects.Length; i++)
            {
                if (_inventoryObjects[i] != null)
                {
                    _savedInventoryData.inventoryObjects.Add(_inventoryObjects[i]);
                    _savedInventoryData.objectsCount.Add(_objectsCount[i]);
                }
            }

            Debug.Log("Inventory saved successfully.");
        }
    }

    public void LoadInventory()
    {
        if (_savedInventoryData != null)
        {
            for (int i = 0; i < _savedInventoryData.inventoryObjects.Count; i++)
            {
                _inventoryObjects[i] = _savedInventoryData.inventoryObjects[i];
                _objectsCount[i] = _savedInventoryData.objectsCount[i];
            }

            OnInventoryUpdated?.Invoke();
            Debug.Log("Inventory loaded successfully.");
        }
    }
    #endregion
}
