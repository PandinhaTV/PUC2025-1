using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlinkEye : MonoBehaviour
{
    [Header("Shape Key / Blink")]
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public int blendShapeIndex = 1; // Conforme sua imagem
    public float minInterval = 3f;  // Conforme sua imagem
    public float maxInterval = 6f;  // Conforme sua imagem
    public float blinkDuration = 0.8f; // Conforme sua imagem

    [Header("Tempo de Olho Fechado")]
    public float closedDuration = 0.5f;

    [Header("Luzes do Olho")]
    public Light[] eyeLights; 
    public Renderer godRayRenderer;
    public float maxLightIntensity = 5f; // Ajuste o brilho aqui!

    private Material godRayMaterial;
    private Color godRayOriginalColor;

    private void Start()
    {
        if (skinnedMeshRenderer == null) return;

        if (godRayRenderer != null)
        {
            godRayMaterial = godRayRenderer.material;
            godRayOriginalColor = godRayMaterial.HasProperty("_Color") ? godRayMaterial.color : Color.white;
        }

        // Garante que comece aberto e com intensidade
        UpdateEyeEffects(1f); 

        StartCoroutine(BlinkRoutine(Random.Range(0f, 2f)));
    }

    private IEnumerator BlinkRoutine(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Fecha o olho (Anima do peso 0 para 100)
            yield return AnimateBlink(0f, 100f);

            yield return new WaitForSeconds(closedDuration);

            // Abre o olho (Anima do peso 100 para 0)
            yield return AnimateBlink(100f, 0f);
        }
    }

    private IEnumerator AnimateBlink(float startWeight, float targetWeight)
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / blinkDuration;
            
            // Suaviza a piscada
            float curveT = Mathf.SmoothStep(0, 1, t);
            float currentWeight = Mathf.Lerp(startWeight, targetWeight, curveT);

            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, currentWeight);

            // Sincroniza a luz com o movimento da pálpebra
            // Se peso é 100 (fechado), visibilidade é 0
            float visibility = 1f - (currentWeight / 100f);
            UpdateEyeEffects(visibility);

            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetWeight);
    }

    private void UpdateEyeEffects(float visibility)
    {
        // Controla as Luzes
        if (eyeLights != null)
        {
            foreach (Light l in eyeLights)
            {
                if (l != null)
                {
                    l.intensity = visibility * maxLightIntensity;
                    l.enabled = l.intensity > 0.01f;
                }
            }
        }

        // Controla o God Ray (Transparência)
        if (godRayMaterial != null)
        {
            Color c = godRayOriginalColor;
            c.a = visibility * godRayOriginalColor.a;
            godRayMaterial.color = c;
        }
    }
}