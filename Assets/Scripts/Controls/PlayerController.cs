using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : IFixedTickable, IInitializable, IDisposable
{
    [Header("Injected Components")]
    private PlayerInput _playerInput;
    private IPlayerStatsProvider _playerStats;
    private IPlayerComponentsProvider _playerComponentsProvider;
    private PlayerConfig _playerConfig;

    [Header("Main Components")]
    private Transform _transform;
    private Rigidbody _rb;
    private Transform _cameraTransform;
    private Transform _rayOriginTransform;

    [Header("Parameters")]
    private float _moveSpeed;
    private float _jumpForce;
    private float _rotationSpeed;
    private float _maxPitch;
    private float _groundCheckDistance;
    private float _minDistanceForJump;
    private float _smoothTime;
    private float _rotationSmoothTime;
    private float _interactionDistance;

    [Header("Temporary Parameters")]
    private Vector2 _movementInput;
    private Vector2 _rotateInput;
    private float _pitch = 0f;
    private float _yaw = 0f;
    private bool _jumpAvailable;
    private Vector3 _currentVelocity = Vector3.zero;
    private float _yawVelocity = 0f;
    private float _pitchVelocity = 0f;

    [Header("Player Actions")]
    public Action<InteractiveObject> OnTocheInteractiveObject;
    public Action<InteractiveObject> OnAimInteractiveObject;
    public Action OnUse;
    public Action OnDrop;
    public Action OnSwitchNext;
    public Action OnSwitchPrevious;

    [Inject]
    public void Construct(PlayerInput playerInput, IPlayerStatsProvider playerStats, IPlayerComponentsProvider playerComponentsProvider, PlayerConfig playerConfig) 
    {
        _playerInput = playerInput;
        _playerComponentsProvider = playerComponentsProvider;
        _playerStats = playerStats;
        _playerConfig = playerConfig;
    }

    void IInitializable.Initialize()
    {
        AddListenersInputSystem();

        AddListenersComponents();

        InitializeComponents();

        InitializeParameters();

        _yaw = _transform.eulerAngles.y;
        _pitch = _cameraTransform.localEulerAngles.x;

        _playerInput.Enable();
    }

    private void AddListenersInputSystem() 
    {
        _playerInput.Main.Jump.performed += context => Jump();
        _playerInput.Main.Interact.performed += context => Interact();
        _playerInput.Main.Use.performed += context => Use();
        _playerInput.Main.Drop.performed += context => Drop();
        _playerInput.Main.Next.performed += context => SwitchNext();
        _playerInput.Main.Previous.performed += context => SwitchPrevious();
    }

    private void AddListenersComponents() 
    {
        _playerStats.OnStatsChanged += OnPlayerStatsChanged;
    }

    private void InitializeComponents() 
    {
        _transform = _playerComponentsProvider.Transform;
        _rb = _playerComponentsProvider.RigidBody;
        _cameraTransform = _playerComponentsProvider.CameraTransform;
        _rayOriginTransform = _playerComponentsProvider.RayOriginTransform;
    }

    private void InitializeParameters()
    {
        _rotationSpeed = _playerConfig.RotationSpeed;
        _maxPitch = _playerConfig.MaxPitch;
        _groundCheckDistance = _playerConfig.GroundCheckDistance;
        _minDistanceForJump = _playerConfig.MinDistanceForJump;
        _smoothTime = _playerConfig.SmoothTime;
        _rotationSmoothTime = _playerConfig.RotationSmoothTime;
        _interactionDistance = _playerConfig.InteractionDistance;
    }

    private void OnPlayerStatsChanged()
    {
        _moveSpeed = _playerStats.Speed;
        _jumpForce = _playerStats.Strength;
    }

    void IDisposable.Dispose()
    {
        _playerStats.OnStatsChanged -= OnPlayerStatsChanged;
        _playerInput.Disable();
    }

    void IFixedTickable.FixedTick()
    {
        Move();
        CheckGround();
        Rotate();
        GetInfo();
    }

    private void Move()
    {
        _movementInput = _playerInput.Main.Move.ReadValue<Vector2>();

        Vector3 desiredVelocity = (_transform.forward * _movementInput.y + _transform.right * _movementInput.x) * _moveSpeed;

        float verticalVelocity = _rb.velocity.y;

        Vector3 smoothedVelocity = Vector3.SmoothDamp(new Vector3(_rb.velocity.x, 0, _rb.velocity.z), desiredVelocity, ref _currentVelocity, _smoothTime);

        _rb.velocity = new Vector3(smoothedVelocity.x, verticalVelocity, smoothedVelocity.z);
    }

    private void Rotate()
    {
        _rotateInput = _playerInput.Main.Rotate.ReadValue<Vector2>();

        float targetYaw = _yaw + _rotateInput.x * _rotationSpeed * Time.deltaTime;
        float targetPitch = _pitch + (-_rotateInput.y * _rotationSpeed * Time.deltaTime);

        _pitch = Mathf.Clamp(targetPitch, -_maxPitch, _maxPitch);

        _yaw = Mathf.SmoothDampAngle(_yaw, targetYaw, ref _yawVelocity, _rotationSmoothTime);

        float smoothedPitch = Mathf.SmoothDampAngle(_cameraTransform.localEulerAngles.x, _pitch, ref _pitchVelocity, _rotationSmoothTime);

        _transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
        _cameraTransform.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
    }

    private void Jump()
    {
        if (_jumpAvailable)
        {
            Debug.Log("Jump");
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(_rayOriginTransform.position, Vector3.down, out hit, _groundCheckDistance))
        {
            _jumpAvailable = hit.distance <= _minDistanceForJump;
        }
    }

    private void Interact()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                Debug.Log("Interact");
                interactable.Interact();
                OnTocheInteractiveObject?.Invoke(interactable);
            }
        }
    }

    private void GetInfo()
    {
        Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            InteractiveObject interactable = hit.collider.GetComponent<InteractiveObject>();
            if (interactable != null)
            {
                //Debug.Log("GetInfo");
                OnAimInteractiveObject?.Invoke(interactable);
            }
        }
    }

    private void Use() 
    {
        Debug.Log("Use");
        OnUse?.Invoke();
    }

    private void Drop()
    {
        Debug.Log("Drop");
        OnDrop?.Invoke();
    }

    private void SwitchNext()
    {
        Debug.Log("SwitchNext");
        OnSwitchNext?.Invoke();
    }

    private void SwitchPrevious()
    {
        Debug.Log("SwitchPrevious");
        OnSwitchPrevious?.Invoke();
    }
}
