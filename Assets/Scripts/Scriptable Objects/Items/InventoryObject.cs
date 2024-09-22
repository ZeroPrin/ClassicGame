using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Item", menuName = "Items/Inventory Item")]
public class InventoryObject : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public GameObject Prefab;
}
