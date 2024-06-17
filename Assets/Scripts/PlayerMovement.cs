using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        IDLE, JOGGING, RUNNING, SNEAKING, JUMPING, FALLING
    }
    
    public PlayerState currentState = PlayerState.IDLE;
    
    public InputAction _moveAction;
    public InputAction _runAction;
    public InputAction _sneakAction;
    public InputAction _jumpAction;
    public Rigidbody _rb;
    private Vector3 _direction = Vector3.zero;

    [Header("Speeds")]
    private float _currentSpeed = 0f;
    public float _joggingSpeed = 5f;
    public float _runningSpeed = 10f;
    public float _sneakSpeed = 2f;

    [Header("Test Bools")]
    public bool _isRunning = false;
    public bool _isSneaking = false;
    public bool _isJumping = false;
    public bool _isGrounded = false;

    public Transform checkGroundTransform;
    public Vector3 checkGroundDimension;
    public LayerMask groundLayer;

    private void OnEnable()
    {
        _moveAction.Enable();
        _runAction.Enable();
        _sneakAction.Enable();
        _jumpAction.Enable();
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        CheckGround();
        OnStateUpdate();
    }

    public void CheckGround()
    {
        Collider[] bodies = Physics.OverlapBox(checkGroundTransform.position, checkGroundDimension, Quaternion.identity, groundLayer);
        _isGrounded |= bodies.Length > 0;
    }

    private void OnDrawGizmos()
    {
        if(_isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(checkGroundTransform.position, checkGroundDimension);
    }

    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.JOGGING:
                _currentSpeed = _joggingSpeed;
                break;
            case PlayerState.RUNNING:
                _currentSpeed = _runningSpeed;
                break;
            case PlayerState.SNEAKING:
                _currentSpeed = _sneakSpeed;
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.FALLING:
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        Vector2 left_stick = _moveAction.ReadValue<Vector2>();

        switch (currentState)
        {
            case PlayerState.IDLE:

                if (_moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    TransitionToState(PlayerState.JOGGING);
                }

                break;
            case PlayerState.JOGGING:

                Move(left_stick, _joggingSpeed);

                if(_moveAction.ReadValue<Vector2>() == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            case PlayerState.RUNNING:

                Move(left_stick, _runningSpeed);

                if(_moveAction.ReadValue<Vector2>() == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }

                break;
            case PlayerState.SNEAKING:

                Move(left_stick, _sneakSpeed);


                break;
            case PlayerState.JUMPING:

                

                break;
            case PlayerState.FALLING:

                

                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.JOGGING:
                break;
            case PlayerState.RUNNING:
                break;
            case PlayerState.SNEAKING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.FALLING:
                break;
            default:
                break;
        }
    }

    void TransitionToState(PlayerState newState)
    {
        OnStateExit();
        currentState = newState;
        OnStateEnter();
    }

    private void Move(Vector2 direction, float speed)
    {
        if(direction.magnitude > 0.1f)
        {
            _rb.velocity = new Vector3(direction.x * speed, _rb.velocity.y, direction.y * speed);
        }
    }


}
