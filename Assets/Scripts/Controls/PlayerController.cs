using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform rayOriginTransform;
    private PlayerInput playerInput;

    [Header("Parameters")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 200f;
    public float maxPitch = 80f;
    public float groundCheckDistance = 1f;
    public float minDistanceForJump = 0.1f;
    public float smoothTime = 0.1f;
    public float rotationSmoothTime = 0.05f;
    public float interactionDistance = 3f;


    private Vector2 movementInput;
    private Vector2 rotateInput;
    private float pitch = 0f;
    private float yaw = 0f;
    private bool jumpAvailable;

    private Vector3 currentVelocity = Vector3.zero;
    private float yawVelocity = 0f;
    private float pitchVelocity = 0f;

    public void Initialize()
    {
        playerInput = new PlayerInput();

        playerInput.Main.Jump.performed += context => Jump();
        playerInput.Main.Interact.performed += context => Interact();
        playerInput.Main.Use.performed += context => Master.HandController.Use();
        playerInput.Main.Drop.performed += context => Master.HandController.Drop();

        yaw = transform.eulerAngles.y;
        pitch = cameraTransform.localEulerAngles.x;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
        Rotate();
        GetInfo();
    }

    public void Move()
    {
        movementInput = playerInput.Main.Move.ReadValue<Vector2>();

        Vector3 desiredVelocity = (transform.forward * movementInput.y + transform.right * movementInput.x) * moveSpeed;

        float verticalVelocity = rb.velocity.y;

        Vector3 smoothedVelocity = Vector3.SmoothDamp(new Vector3(rb.velocity.x, 0, rb.velocity.z), desiredVelocity, ref currentVelocity, smoothTime);

        rb.velocity = new Vector3(smoothedVelocity.x, verticalVelocity, smoothedVelocity.z);
    }

    public void Rotate()
    {
        rotateInput = playerInput.Main.Rotate.ReadValue<Vector2>();

        float targetYaw = yaw + rotateInput.x * rotationSpeed * Time.deltaTime;
        float targetPitch = pitch + (-rotateInput.y * rotationSpeed * Time.deltaTime);

        pitch = Mathf.Clamp(targetPitch, -maxPitch, maxPitch);

        yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawVelocity, rotationSmoothTime);

        float smoothedPitch = Mathf.SmoothDampAngle(cameraTransform.localEulerAngles.x, pitch, ref pitchVelocity, rotationSmoothTime);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cameraTransform.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
    }

    public void Jump()
    {
        if (jumpAvailable)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOriginTransform.position, Vector3.down, out hit, groundCheckDistance))
        {
            jumpAvailable = hit.distance <= minDistanceForJump;
        }
    }

    public void Interact()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                if (interactable is Item) 
                {
                    Master.Inventory.AddItem(interactable.GetComponent<Item>());
                }

                interactable.Interact();
            }
        }
    }

    public void GetInfo()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                interactable.GetInfo();
            }
        }
    }
}
