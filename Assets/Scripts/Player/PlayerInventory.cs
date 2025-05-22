using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<InventoryItemData> collectedItems = new();

    public void AddItem(InventoryItemData item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            Debug.Log($"Added: {item.itemName}");
            InventoryUIManager.Instance?.RefreshUI();
        }
    }

    public bool HasItem(InventoryItemData item)
    {
        return collectedItems.Contains(item);
    }

    public T GetItem<T>() where T : InventoryItemData
    {
        foreach (var item in collectedItems)
        {
            if (item is T matched)
                return matched;
        }
        return null;
    }

    public IEnumerable<InventoryItemData> GetAllItems() => collectedItems;

    public void RemoveItem(InventoryItemData item)
    {
        if (collectedItems.Contains(item))
        {
            collectedItems.Remove(item);
            InventoryUIManager.Instance?.RefreshUI();
        }
    }

}



public abstract class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
}