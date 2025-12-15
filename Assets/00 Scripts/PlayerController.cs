using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction;
    public float interactRadius = 3f;
    public LayerMask interactableMask;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    public InputActionReference sprintAction;

    [Header("Camera")]
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("UI")]
    public InteractionUI interactionUI;

    private IInteractable currentTarget;

    void Awake() => controller = GetComponent<CharacterController>();

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();
        interactAction.action.Enable();
        
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
        interactAction.action.Disable();
    }

    void Update()
    {
        if (sprintAction.action.IsInProgress())
        {
            moveSpeed = 7.5f;
        }
        else
        {
            moveSpeed = 5f;
        }
        DetectNearbyInteractables();
        HandleInteraction();

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Ignore tiny inputs
        if (input.sqrMagnitude < 0.01f)
        {
            ApplyGravityAndJump();
            return;
        }

        // ✅ Calculate target angle based on camera orientation and input direction
        float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        // Smoothly rotate the player
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Move in the direction the character is facing
        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

        ApplyGravityAndJump();
    }

    private float rotationVelocity; // Used for smooth turning

    void ApplyGravityAndJump()
    {
        
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void DetectNearbyInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius, interactableMask);
        IInteractable closest = null;
        float closestDist = float.MaxValue;

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    
                    closest = interactable;
                    closestDist = dist;
                }
            }
        }

        if (closest != currentTarget)
        {
            
            currentTarget = closest;
            if (currentTarget != null)
                interactionUI.ShowPrompt(currentTarget.GetPromptText(), (currentTarget as MonoBehaviour).transform);
            else
                interactionUI.HidePrompt();
        }
    }

    void HandleInteraction()
    {
        if (currentTarget != null && interactAction.action.WasPressedThisFrame())
            currentTarget.Interact(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
