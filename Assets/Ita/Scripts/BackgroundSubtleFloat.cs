using UnityEngine;

public class BackgroundSubtleFloat : MonoBehaviour
{
    public float amplitude = 0.06f;
    public float speed = 0.5f;
    public float rotationAmount = 1.2f;

    [Range(1, 5)]
    public int updateEveryXFrames = 2;

    private Vector3 startPos;
    private Quaternion startRot;
    private float offset;
    private int frameOffset;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        offset = Random.Range(0f, 100f);
        frameOffset = Random.Range(0, updateEveryXFrames);
    }

    void Update()
    {
        if ((Time.frameCount + frameOffset) % updateEveryXFrames != 0)
            return;

        float t = Time.time + offset;

        float y = Mathf.Sin(t * speed + Mathf.PerlinNoise(offset, t)) * amplitude;
        transform.position = startPos + Vector3.up * y;

        float z = Mathf.Sin(t * speed * 0.7f) * rotationAmount;
        transform.rotation = startRot * Quaternion.Euler(0f, 0f, z);
    }
}
