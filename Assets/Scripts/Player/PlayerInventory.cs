using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<KeyData> collectedKeys = new List<KeyData>();
    public List<BatteryData> collectedBatteries = new List<BatteryData>();

    public void AddKey(KeyData key)
    {
        if (!collectedKeys.Contains(key))
        {
            collectedKeys.Add(key);
            InventoryUIManager.Instance?.RefreshUI();
        }
    }

    public void AddBattery(BatteryData battery)
    {
        collectedBatteries.Add(battery);
        InventoryUIManager.Instance?.RefreshUI();
    }
    
    public bool HasKey(KeyData key)
    {
        return collectedKeys.Contains(key);
    }
}