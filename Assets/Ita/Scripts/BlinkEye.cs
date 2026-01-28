using UnityEngine;
using System.Collections;

public class BlinkEyeDebug : MonoBehaviour
{
    [Header("Shape Key / Blink")]
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int blendShapeIndex = 0;
    public float minInterval = 2f;
    public float maxInterval = 5f;
    public float blinkDuration = 0.15f; 

    [Header("Tempo de Olho Fechado")]
    public float closedDuration = 0.5f;

    [Header("Componentes de Iluminação")]
    public Light eyeLight;
    public Renderer godRayRenderer; 
    public float maxLightIntensity = 5f;

    private Material godRayMaterial;
    private Color originalColor;

    private void Start()
    {
        if (skinnedMeshRenderer == null) return;

        // Captura o material de forma segura
        if (godRayRenderer != null)
        {
            godRayMaterial = godRayRenderer.material;
            originalColor = godRayMaterial.color;
        }

        // Garante que a luz comece acesa
        if (eyeLight != null) eyeLight.intensity = maxLightIntensity;

        StartCoroutine(BlinkRoutine(Random.Range(0f, 1f)));
    }

    private IEnumerator BlinkRoutine(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Fecha e apaga
            yield return AnimateBlink(100f);

            yield return new WaitForSeconds(closedDuration);

            // Abre e acende
            yield return AnimateBlink(0f);
        }
    }

    private IEnumerator AnimateBlink(float targetWeight)
    {
        float startWeight = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / blinkDuration);
            float currentWeight = Mathf.Lerp(startWeight, targetWeight, t);

            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentWeight);

            // VISIBILIDADE: 1 = Aberto/Aceso, 0 = Fechado/Apagado
            float visibility = 1f - (currentWeight / 100f);

            // Atualiza a Luz
            if (eyeLight != null)
            {
                eyeLight.intensity = visibility * maxLightIntensity;
                // Força a luz a ficar ativa se a visibilidade for maior que 0
                eyeLight.enabled = (eyeLight.intensity > 0.01f);
            }

            // Atualiza o God Ray
            if (godRayMaterial != null)
            {
                Color c = originalColor;
                c.a = visibility * originalColor.a;
                godRayMaterial.color = c;
            }

            yield return null;
        }

        // Ajuste Final de Segurança
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
        if (eyeLight != null) 
        {
            eyeLight.intensity = (targetWeight <= 0) ? maxLightIntensity : 0f;
            eyeLight.enabled = (targetWeight <= 0);
        }
    }
}