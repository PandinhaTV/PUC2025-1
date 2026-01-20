using UnityEngine;
using System.Collections;

public class BlinkEyeDebug : MonoBehaviour
{
    [Header("Shape Key / Blink")]
    public SkinnedMeshRenderer skinnedMeshRenderer; // arraste o mesh aqui
    public int blendShapeIndex = 0;                 // índice do shape key do olho
    public float minInterval = 2f;                  // intervalo mínimo entre piscadas
    public float maxInterval = 5f;                  // intervalo máximo entre piscadas
    public float blinkDuration = 0.1f;              // duração do fechamento/abertura

    [Header("Eye Light (Farol)")]
    public Light eyeLight;                          // Spot ou Point Light no olho
    public float openIntensity = 5f;                // intensidade máxima quando aberto

    private void Start()
    {
        if (skinnedMeshRenderer == null)
        {
            Debug.LogError("SkinnedMeshRenderer não atribuído!");
            enabled = false;
            return;
        }

        // Garantia inicial
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, 0f);

        if (eyeLight != null)
            eyeLight.intensity = 0f;

        StartCoroutine(BlinkRoutine(Random.Range(0f, 1f)));
    }

    private IEnumerator BlinkRoutine(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Fecha
            yield return AnimateBlink(100f);

            // Abre
            yield return AnimateBlink(0f);
        }
    }

    private IEnumerator AnimateBlink(float target)
    {
        float start = skinnedMeshRenderer.GetBlendShapeWeight(blendShapeIndex);
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / blinkDuration);
            float value = Mathf.Lerp(start, target, t);

            skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, value);

            // 🔑 CONTROLE ABSOLUTO DA LUZ
            if (eyeLight != null)
            {
                if (value >= 99f)
                {
                    eyeLight.intensity = 0f;
                }
                else
                {
                    eyeLight.intensity = (1f - value / 100f) * openIntensity;
                }
            }

            yield return null;
        }

        // Garantia final (sem risco de frame errado)
        skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, target);

        if (eyeLight != null)
            eyeLight.intensity = target >= 100f ? 0f : openIntensity;
    }
}
