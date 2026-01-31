using TMPro;
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
    public float moveSpeed = 1.3f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("UI")]
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    
[Header("Animator")]
public Animator animator;
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
            
            animator.SetBool("IsRunning", true);
            moveSpeed = 7.5f;
        }
        else
        {
            
            animator.SetBool("IsRunning", false);
            moveSpeed = 5f;
        }
        InteractionSphere();

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
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

       //if (jumpAction.action.WasPressedThisFrame() && isGrounded)
          //  velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    

    void InteractionSphere()
    {
        bool hitSomething = false;

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            interactRadius
        );

        for (int i = 0; i < hits.Length; i++)
        {
            IInteractable interactable = hits[i].GetComponent<IInteractable>();
            if (interactable != null)
            {
                hitSomething = true;
                interactionText.text = interactable.GetDescription();

                if (interactAction.action.WasPressedThisFrame())
                {
                    interactable.Interact();
                }

                break; // stop after the first valid interactable
            }
        }

        interactionUI.SetActive(hitSomething);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
