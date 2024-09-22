using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "ScriptableObjects/InventoryData", order = 1)]
public class InventoryData : ScriptableObject
{
    public List<InventoryObject> inventoryObjects = new List<InventoryObject>();
    public List<int> objectsCount = new List<int>();
}
