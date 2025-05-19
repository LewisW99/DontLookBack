using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light flickerLight;
    public float noiseSpeed = 1f;
    public float intensityBase = 1f;
    public float intensityVariation = 0.5f;

    private float noiseOffset;

    private void Start()
    {
        flickerLight = GetComponent<Light>();
        noiseOffset = Random.Range(0f, 100f); // avoid synchronized flicker
    }

    private void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * noiseSpeed + noiseOffset, 0f);
        flickerLight.intensity = intensityBase + noise * intensityVariation;
    }
}
