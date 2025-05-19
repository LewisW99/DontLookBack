using UnityEngine;

public class FlashlightBattery : MonoBehaviour
{
    [Header("Battery Settings")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float decayRate = 10f;
    public float rechargeRate = 5f;

    [Header("Recharge Settings")]
    public bool canRecharge;

    [Header("Low Battery")]
    public float lowBatteryThreshold = 35f;
    public float lowBatterySpotAngle = 20f;
    public float minIntensity = 0.2f;

    [Header("Controls")]
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    [SerializeField] private FlashlightController flashlightController;

    private float originalSpotAngle;
    private float originalIntensity;
    private bool valuesStored;

    private void Start()
    {
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    private void Update()
    {
        if (flashlightController == null || flashlightController.flashlight == null)
            return;

        if (flashlightController.IsOn())
        {
            StoreInitialLightValues();
            DrainBattery();
            HandleLowBatteryEffects();
        }
        else
        {
            RestoreOriginalLightValues();

            if (canRecharge && currentBattery < maxBattery)
                RechargeBattery();

            if (Input.GetKeyDown(reloadKey))
                TryReloadBattery();
        }

        CheckBatteryStatus();
    }

    private void StoreInitialLightValues()
    {
        if (valuesStored) return;

        originalSpotAngle = flashlightController.flashlight.spotAngle;
        originalIntensity = flashlightController.flashlight.intensity;
        valuesStored = true;
    }

    private void RestoreOriginalLightValues()
    {
        if (!valuesStored) return;

        flashlightController.flashlight.spotAngle = originalSpotAngle;
        flashlightController.flashlight.intensity = originalIntensity;
        valuesStored = false;
    }

    private void DrainBattery()
    {
        currentBattery -= decayRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
        Debug.Log($"Battery: {Mathf.RoundToInt(currentBattery)}%");

        if (currentBattery <= 0f)
        {
            flashlightController.ForceOff();
            flashlightController.UseBattery();
        }
    }

    private void RechargeBattery()
    {
        currentBattery += rechargeRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    private void TryReloadBattery()
    {
        PlayerInventory inventory = flashlightController.GetComponent<PlayerInventory>();
        if (inventory == null || inventory.collectedBatteries.Count == 0)
        {
            Debug.Log("No spare batteries.");
            return;
        }

        // Remove first battery (or let player choose in future)
        BatteryData battery = inventory.collectedBatteries[0];
        inventory.collectedBatteries.RemoveAt(0);

        currentBattery = Mathf.Clamp(currentBattery + battery.restoreAmount, 0f, maxBattery);
        Debug.Log("Battery reloaded with " + battery.displayName);
        InventoryUIManager.Instance?.RefreshUI();
    }

    private void CheckBatteryStatus()
    {
        // Future hook for warning effects
    }

    private void HandleLowBatteryEffects()
    {
        if (currentBattery > lowBatteryThreshold)
        {
            RestoreOriginalLightValues();
            return;
        }

        float t = Mathf.InverseLerp(0f, lowBatteryThreshold, currentBattery);
        flashlightController.flashlight.spotAngle = Mathf.Lerp(lowBatterySpotAngle, originalSpotAngle, t);
        flashlightController.flashlight.intensity = Mathf.Lerp(minIntensity, originalIntensity, t);
    }

    // External recharge (e.g. from pickup)
    public void AddBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);
    }

    public float GetBatteryPercentage() => (currentBattery / maxBattery) * 100f;
    public bool IsBatteryLow() => currentBattery <= lowBatteryThreshold;
}
