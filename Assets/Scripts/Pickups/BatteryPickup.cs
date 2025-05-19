using UnityEngine;

public class BatteryPickup : Pickup
{
    public float batteryAmount = 25f;

    public override void OnPickup(GameObject player)
    {
        FlashlightBattery battery = player.GetComponentInChildren<FlashlightBattery>();
        if (battery != null)
        {
            battery.AddBattery(batteryAmount);
        }
    }
}