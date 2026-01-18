using UnityEngine;
using System.Collections;

public class BlinkEyeDebug : MonoBehaviour
{
    [Header("Shape Key / Blink")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // arraste seu mesh aqui
    public int blendShapeIndex = 0; // índice do shape key do olho
    public float minInterval = 2f; // intervalo mínimo entre piscadas
    public float maxInterval = 5f; // intervalo máximo entre piscadas
    public float blinkDuration = 0.1f; // duração do fechamento/abertura

    [Header("Eye Light (Farol)")]
    public Light eyeLight; // assign a Spot Light ou Point Light no olho
    public float openIntensity = 5f; // intensidade máxima do “farol” quando aberto
    public float closedIntensity = 0f; // intensidade quando fechado
    public float lightFadeSpeed = 5f; // quão rápido a luz sobe/desce ao abrir/fechar

    private void Start()
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer não está atribuído!");
            return;
        }

        if (eyeLight != null)
            eyeLight.intensity = closedIntensity; // garante que começa apagado

        StartCoroutine(BlinkRoutine(Random.Range(0f, 1f))); // offset inicial
    }

    private IEnumerator BlinkRoutine(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // Fecha o olho
            yield return AnimateBlendShape(100f, blinkDuration);
            if (eyeLight != null) StartCoroutine(FadeLight(closedIntensity, blinkDuration));

            // Abre o olho
            yield return AnimateBlendShape(0f, blinkDuration);
            if (eyeLight != null) StartCoroutine(FadeLight(openIntensity, blinkDuration));
        }
    }

    private IEnumerator AnimateBlendShape(float targetValue, float duration)
    {
        float startValue = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(startValue, targetValue, t);
            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, value);
            yield return null;
        }

        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, targetValue);
    }

    private IEnumerator FadeLight(float targetIntensity, float duration)
    {
        float startIntensity = eyeLight.intensity;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            eyeLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            yield return null;
        }

        eyeLight.intensity = targetIntensity;
    }
}
