using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light flashlight;

    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] bool isOn;

    [SerializeField] private int batteryCount = 1;

    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false; // Ensure flashlight is off at start
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }

    public void ForceOff()
    {
        isOn = false;
        if (flashlight != null)
            flashlight.enabled = false;
    }

    public bool IsOn() => flashlight != null && flashlight.enabled;

    public void AddBattery()
    {
        batteryCount++;
        Debug.Log("Battery added. Total batteries: " + batteryCount);
    }

    public bool UseBattery()
    {
        if (batteryCount > 0)
        {
            batteryCount--;
            return true;
        }
        return false;
    }

    public int GetBatteryCount() => batteryCount;
}
