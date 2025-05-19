using UnityEngine;

public class BatteryPickup : Pickup
{
    [SerializeField] private BatteryData batteryData;

    public override void OnPickup(GameObject player)
    {
        var inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddBattery(batteryData);
            Debug.Log("Picked up battery: " + batteryData.displayName);
            if (batteryData.icon == null)
            {
                Debug.Log("icon is null");
            }
          
        }
    }
}