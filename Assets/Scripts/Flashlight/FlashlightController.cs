using UnityEngine;
// ReSharper disable CheckNamespace

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private Light flashlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    public bool isOn;
    void Start()
    {
        if (flashlight != null)
            flashlight.enabled = false; // Ensure it's off at start
    }
    
    public void ForceOff()
    {
        isOn = false;
        flashlight.enabled = false;
    }

    public bool IsOn() => flashlight.enabled;
    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight != null)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}