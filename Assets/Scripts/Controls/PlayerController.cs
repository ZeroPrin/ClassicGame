using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Header("Main Components")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _rayOriginTransform;

    private PlayerStats _playerStats;
    private PlayerInput _playerInput;
    private HandController _handController;
    private Inventory _inventory;

    [Header("Parameters")]
    public float MoveSpeed = 5f;
    public float JumpForce = 5f;
    public float RotationSpeed = 200f;
    public float MaxPitch = 80f;
    public float GroundCheckDistance = 1f;
    public float MinDistanceForJump = 0.1f;
    public float SmoothTime = 0.1f;
    public float RotationSmoothTime = 0.05f;
    public float InteractionDistance = 3f;

    [Header("Temporary Parameters")]
    private Vector2 _movementInput;
    private Vector2 _rotateInput;
    private float _pitch = 0f;
    private float _yaw = 0f;
    private bool _jumpAvailable;
    private Vector3 _currentVelocity = Vector3.zero;
    private float _yawVelocity = 0f;
    private float _pitchVelocity = 0f;

    [Inject]
    public void Construct(PlayerInput playerInput, PlayerStats playerStats, HandController handController, Inventory inventory) 
    {
        _playerInput = playerInput;
        _playerStats = playerStats;
        _handController = handController;
        _inventory = inventory;
    }

    public void Initialize()
    {
        _playerInput.Main.Jump.performed += context => Jump();
        _playerInput.Main.Interact.performed += context => Interact();
        _playerInput.Main.Use.performed += context => _handController.Use();
        _playerInput.Main.Drop.performed += context => _handController.Drop();
        _playerInput.Main.Next.performed += context => _inventory.SwitchNext();
        _playerInput.Main.Previous.performed += context => _inventory.SwitchPrevious();

        _yaw = transform.eulerAngles.y;
        _pitch = _cameraTransform.localEulerAngles.x;

        _playerStats.OnStatsChanged += SetStats;

        _playerInput.Enable();
    }

    public void Deinitialize() 
    {
        _playerInput.Disable();
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
        _movementInput = _playerInput.Main.Move.ReadValue<Vector2>();

        Vector3 desiredVelocity = (transform.forward * _movementInput.y + transform.right * _movementInput.x) * MoveSpeed;

        float verticalVelocity = _rb.velocity.y;

        Vector3 smoothedVelocity = Vector3.SmoothDamp(new Vector3(_rb.velocity.x, 0, _rb.velocity.z), desiredVelocity, ref _currentVelocity, SmoothTime);

        _rb.velocity = new Vector3(smoothedVelocity.x, verticalVelocity, smoothedVelocity.z);
    }

    public void Rotate()
    {
        _rotateInput = _playerInput.Main.Rotate.ReadValue<Vector2>();

        float targetYaw = _yaw + _rotateInput.x * RotationSpeed * Time.deltaTime;
        float targetPitch = _pitch + (-_rotateInput.y * RotationSpeed * Time.deltaTime);

        _pitch = Mathf.Clamp(targetPitch, -MaxPitch, MaxPitch);

        _yaw = Mathf.SmoothDampAngle(_yaw, targetYaw, ref _yawVelocity, RotationSmoothTime);

        float smoothedPitch = Mathf.SmoothDampAngle(_cameraTransform.localEulerAngles.x, _pitch, ref _pitchVelocity, RotationSmoothTime);

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        _cameraTransform.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
    }

    public void Jump()
    {
        if (_jumpAvailable)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(_rayOriginTransform.position, Vector3.down, out hit, GroundCheckDistance))
        {
            _jumpAvailable = hit.distance <= MinDistanceForJump;
        }
    }

    public void Interact()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                if (interactable is Item) 
                {
                    _inventory.AddItem(interactable.GetComponent<Item>());
                }

                interactable.Interact();
            }
        }
    }

    public void GetInfo()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                interactable.GetInfo();
            }
        }
    }

    public void SetStats() 
    {
        MoveSpeed = _playerStats.Speed;
        JumpForce = _playerStats.Strength;
    }
}
