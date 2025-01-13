using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class MixamoCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2.0f;
    public float jumpForce = 4.0f;
    public float gravity = -9.8f;

    [Header("Respawn Settings")]
    public Transform respawnPoint;

    private CharacterController _characterController;
    private Animator _animator;
    private Vector3 _velocity;
    private bool _isJumping;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _motionInput;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
    }

    void Update()
    {
        if (!_characterController.enabled) return;

        GetLocomotionInput();
        HandleMovement();
        HandleJumping();

        // Yerçekimi uygulama
        if (!_characterController.isGrounded)
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _characterController.Move(_velocity * Time.deltaTime);

        PreventFalling();
    }


    private void GetLocomotionInput()
    {
        var hInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        var vInput = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
        _motionInput = new Vector2(hInput, vInput);
    }

    private void HandleMovement()
    {
        if (!_characterController.enabled) return;

        Vector3 moveDirection = new Vector3(_motionInput.x, 0, _motionInput.y).normalized;
        float speed = moveDirection.magnitude * moveSpeed;
        _animator.SetFloat("Speed", speed);

        if (speed > 0.1f)
        {
            transform.forward = moveDirection;
        }

        _velocity.x = moveDirection.x * moveSpeed;
        _velocity.z = moveDirection.z * moveSpeed;

        if (_characterController.isGrounded)
        {
            _velocity.y = -0.1f;
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleJumping()
    {
        if (_characterController.isGrounded && _jumpAction.triggered)
        {
            Debug.Log("Jumping!");
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }


    private void PreventFalling()
    {
        if (transform.position.y < -10f)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        if (respawnPoint != null)
        {
            _characterController.enabled = false;
            transform.position = respawnPoint.position;
            _velocity = Vector3.zero;
            _characterController.enabled = true;
        }
    }
}
