using UnityEngine;

public class LightFlickerHybrid : MonoBehaviour
{
    Light l;
    float seed;

    public float baseIntensity = 8f;
    public float intensityVariation = 2f;

    public float baseRange = 10f;
    public float rangeVariation = 2f;

    public float flickerSpeed = 3f;

    void Awake()
    {
        l = GetComponent<Light>();
        seed = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(seed, Time.unscaledTime * flickerSpeed);

        l.intensity = baseIntensity + noise * intensityVariation;
        l.range     = baseRange     + noise * rangeVariation;
    }
}
