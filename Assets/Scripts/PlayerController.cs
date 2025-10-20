using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference interactAction; // E / Gamepad X
    public float interactRadius = 3f;
    public LayerMask interactableMask;
    public InputActionReference moveAction;
    public InputActionReference jumpAction;
    [Header("Camera")]
    public Transform cameraTransform;
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("UI")]
    public InteractionUI interactionUI; // Reference to your UI script

    private IInteractable currentTarget;
    void Awake() => controller = GetComponent<CharacterController>();

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        interactAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        interactAction.action.Disable();
    }

    void Update()
    {
        

        DetectNearbyInteractables();
        HandleInteraction();
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Camera-relative movement direction
        Vector3 move = cameraTransform.forward * input.y + cameraTransform.right * input.x;
        move.y = 0f;
        move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);

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
            {
                // Show prompt above object
                interactionUI.ShowPrompt(currentTarget.GetPromptText(), (currentTarget as MonoBehaviour).transform);
            }
            else
            {
                interactionUI.HidePrompt();
            }
        }
    }

    void HandleInteraction()
    {
        if (currentTarget != null && interactAction.action.WasPressedThisFrame())
        {
            currentTarget.Interact(gameObject);
        }
    }

    // Optional: visualize the radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
    
}