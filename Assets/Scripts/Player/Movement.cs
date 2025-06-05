using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] private float _shiftMultiplier = 2f;
    [SerializeField] private float _jumpForce = 500f;
    [SerializeField] private AudioClip _walkingSound;
    [SerializeField] private float _stepInterval = 0.5f;
    [SerializeField] private float _sprintStepIntervalMultiplier = 0.7f;
    [Range(0f, 1f)]
    [SerializeField] private float _baseVolume = 0.5f;  // Базовая громкость
    [Range(0f, 1f)]
    [SerializeField] private float _sprintVolumeMultiplier = 1f; // Увеличение громкости при беге
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private float _currentSpeed;
    private AudioSource _audioSource;
    private float _lastStepTime = 0f;
    private float _currentStepInterval;
    private float _currentVolume;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }
        if (_walkingSound == null)
        {
            Debug.LogWarning("звук ходьбы не назначен");
        }
        _audioSource.clip = _walkingSound;
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.volume = _baseVolume;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _currentSpeed = _baseSpeed;
        _currentStepInterval = _stepInterval;
        _currentVolume = _baseVolume;
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleGravity();
        HandleWalkingSound();
    }

    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        _currentSpeed = _baseSpeed;
        if (moveDirection.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed *= _shiftMultiplier;
        }
        _controller.Move(moveDirection * _currentSpeed * Time.deltaTime);
    }


    private void HandleWalkingSound()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        if (moveDirection.magnitude > 0 && _isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _currentStepInterval = _stepInterval * _sprintStepIntervalMultiplier;
                _currentVolume = _baseVolume * _sprintVolumeMultiplier;
            }
            else
            {
                _currentStepInterval = _stepInterval;
                _currentVolume = _baseVolume;
            }
            if (Time.time - _lastStepTime > _currentStepInterval)
            {
                _lastStepTime = Time.time;
                _audioSource.volume = _currentVolume;
                _audioSource.PlayOneShot(_walkingSound);
            }
        }
    }
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }
    }
    private void HandleGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, _groundLayer);
    }
}