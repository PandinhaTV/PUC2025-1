using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference lookAction;       // Mouse or right stick (Vector2)
    public InputActionReference switchShoulder;   // Button to swap shoulder view

    [Header("Target")]
    public Transform target;                      // Player or follow target

    [Header("Camera Settings")]
    public float distance = 4f;
    public float height = 1.5f;
    public float shoulderOffset = 1.2f;
    public float sensitivity = 1.5f;
    public float rotationSmoothTime = 0.05f;
    public float positionSmoothTime = 0.1f;
    public float minPitch = -30f;
    public float maxPitch = 70f;
    public float autoAlignDelay = 3f;             // Seconds before auto-align starts
    public float autoAlignSpeed = 2f;             // How fast the camera realigns

    private float yaw;
    private float pitch;
    private bool rightShoulder = true;
    private float shoulderSide = 1f;              // For smooth interpolation
    private float currentShoulderSide = 1f;

    private Vector3 currentVelocity;
    private Quaternion currentRotation;
    private float lastLookInputTime;

    void OnEnable()
    {
        lookAction.action.Enable();
        if (switchShoulder != null) switchShoulder.action.Enable();
    }

    void OnDisable()
    {
        lookAction.action.Disable();
        if (switchShoulder != null) switchShoulder.action.Disable();
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleCameraInput();
        UpdateCameraPosition();
    }

    void HandleCameraInput()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        // Detect look input activity
        if (lookInput.sqrMagnitude > 0.01f)
        {
            yaw += lookInput.x * sensitivity;
            pitch -= lookInput.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            lastLookInputTime = Time.time;
        }

        // Shoulder switching
        if (switchShoulder != null && switchShoulder.action.WasPressedThisFrame())
        {
            rightShoulder = !rightShoulder;
        }

        // Smooth shoulder offset transition
        float targetSide = rightShoulder ? 1f : -1f;
        currentShoulderSide = Mathf.Lerp(currentShoulderSide, targetSide, Time.deltaTime * 5f);

        // Auto-align camera behind player after inactivity
        if (Time.time - lastLookInputTime > autoAlignDelay)
        {
            // Get the target's facing direction on the Y axis
            float targetYaw = target.eulerAngles.y;
            yaw = Mathf.LerpAngle(yaw, targetYaw, Time.deltaTime * autoAlignSpeed);
        }
    }

    void UpdateCameraPosition()
    {
        // Calculate rotation from yaw/pitch
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate desired position with height, distance, and shoulder offset
        Vector3 sideOffset = targetRotation * Vector3.right * shoulderOffset * currentShoulderSide;
        Vector3 desiredPosition =
            target.position
            - targetRotation * Vector3.forward * distance
            + Vector3.up * height
            + sideOffset;

        // Smooth position & rotation to avoid jagginess
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, positionSmoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
    }
}
