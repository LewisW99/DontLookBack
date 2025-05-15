using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private FlashlightBattery flashlightBattery;

    [Header("UI Elements")]
    [SerializeField] private Slider batterySlider;
    [SerializeField] private TextMeshProUGUI percentageText; // Optional
    [SerializeField] private Image fillImage;     // Assign this to the slider's Fill image

    [Header("Colors")]
    public Color fullColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color lowColor = Color.red;
    public float lowThreshold = 0.25f;
    public float mediumThreshold = 0.5f;

    void Update()
    {
        if (flashlightBattery == null || batterySlider == null || fillImage == null) return;

        float percent = flashlightBattery.GetBatteryPercentage();
        float normalized = percent / 100f;

        batterySlider.value = normalized;

        // Set color
        if (normalized <= lowThreshold)
        {
            fillImage.color = lowColor;
        }
        else if (normalized <= mediumThreshold)
        {
            fillImage.color = mediumColor;
        }
        else
        {
            fillImage.color = fullColor;
        }

        // Optional % text
        if (percentageText != null)
        {
            percentageText.text = Mathf.RoundToInt(percent) + "%";
        }
    }
}
