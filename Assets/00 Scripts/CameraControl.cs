using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [Header("Collision")] public LayerMask collisionMask; // Layers considered obstacles (e.g., Environment)
    public float cameraRadius = 0.3f; // Prevents camera clipping through corners
    public float collisionSmoothTime = 0.05f;
    private float currentDistance;
    private Vector3 desiredCameraPosition;

    [Header("Input")] public InputActionReference lookAction; // Mouse or right stick (Vector2)
    public InputActionReference switchShoulder; // Button to swap shoulder view

    [Header("Target")] public Transform target; // Player or follow target

    [Header("Camera Settings")] public float distance = 4f;
    public float height = 1.5f;
    public float shoulderOffset = 1.2f;
    public float sensitivity = 1.5f;
    public float rotationSmoothTime = 0.05f;
    public float positionSmoothTime = 0.1f;
    public float minPitch = -30f;
    public float maxPitch = 70f;
    public float autoAlignDelay = 3f; // Seconds before auto-align starts
    public float autoAlignSpeed = 2f; // How fast the camera realigns

    private float yaw;
    private float pitch;
    private bool rightShoulder = true;
    private float shoulderSide = 1f; // For smooth interpolation
    private float currentShoulderSide = 1f;

    private Vector3 currentVelocity;
    private Quaternion currentRotation;
    private float lastLookInputTime;

    void Start()
    {
        currentDistance = distance;
    }

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
        
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);

            // Shoulder offset (smooth side switch)
            Vector3 sideOffset = targetRotation * Vector3.right * shoulderOffset * currentShoulderSide;

            // Target camera pivot point (roughly behind the head)
            Vector3 pivot = target.position + Vector3.up * height;

            // Desired camera position (without collision)
            Vector3 desiredPos = pivot - targetRotation * Vector3.forward * distance + sideOffset;

            // --- Step 1: SphereCast from player to camera to detect obstacles ---
            Vector3 direction = (desiredPos - pivot).normalized;
            float targetDistance = distance;

            if (Physics.SphereCast(pivot, cameraRadius, direction, out RaycastHit hit, distance, collisionMask,
                    QueryTriggerInteraction.Ignore))
            {
                targetDistance = Mathf.Clamp(hit.distance - 0.05f, 0.2f, distance);
            }

            // --- Step 2: Check if final position overlaps anything ---
            Vector3 adjustedPos = pivot - targetRotation * Vector3.forward * targetDistance + sideOffset;
            Collider[] overlaps =
                Physics.OverlapSphere(adjustedPos, cameraRadius, collisionMask, QueryTriggerInteraction.Ignore);
            if (overlaps.Length > 0)
            {
                // Push camera forward slightly until it's clear
                float safeDistance = targetDistance;
                for (int i = 0; i < 5; i++) // iterative safety push
                {
                    safeDistance -= 0.05f;
                    Vector3 testPos = pivot - targetRotation * Vector3.forward * safeDistance + sideOffset;
                    if (Physics.OverlapSphere(testPos, cameraRadius, collisionMask, QueryTriggerInteraction.Ignore)
                            .Length == 0)
                    {
                        adjustedPos = testPos;
                        break;
                    }
                }
            }

            // --- Step 3: Smooth distance change and apply final transform ---
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime / collisionSmoothTime);
            transform.position =
                Vector3.SmoothDamp(transform.position, adjustedPos, ref currentVelocity, positionSmoothTime);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
        
    }
}