using UnityEngine;

public class FlashlightBattery : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float decayRate = 10f; // units per second
    public float rechargeRate = 5f; // optional

    [Header("Optional Recharge Settings")]
    public bool canRecharge;

    [Header("Low Battery Threshold")]
    public float lowBatteryThreshold = 20f;

    [SerializeField] private FlashlightController flashlightController;

    private void Start()
    {
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    void Update()
    {
        if (flashlightController.IsOn())
        {
            DrainBattery();
        }
        else if (canRecharge && currentBattery < maxBattery)
        {
            RechargeBattery();
        }

        CheckBatteryStatus();
    }

    private void DrainBattery()
    {
        currentBattery -= decayRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        Debug.Log($" Battery: {Mathf.RoundToInt(currentBattery)}%");

        if (currentBattery <= 0f)
        {
            flashlightController.ForceOff(); // safer way than disabling script
        }
    }

    private void RechargeBattery()
    {
        currentBattery += rechargeRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    private void CheckBatteryStatus()
    {
        // Could be used to trigger flicker or warning effects
        if (currentBattery <= lowBatteryThreshold)
        {
            // You can notify a flicker system here
            // Example: EventManager.TriggerLowBatteryWarning();
        }
    }

    // Call this externally if you want to recharge with items
    public void AddBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    public float GetBatteryPercentage()
    {
        return (currentBattery / maxBattery) * 100f;
    }

    public bool IsBatteryLow()
    {
        return currentBattery <= lowBatteryThreshold;
    }
}
