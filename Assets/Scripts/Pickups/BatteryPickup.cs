using UnityEngine;

public class BatteryPickup : Pickup
{
    public override void OnPickup(GameObject player)
    {
        FindFirstObjectByType<FlashlightController>().AddBattery();
    }
}
