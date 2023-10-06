using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _speed;
    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] PlayerInput _playerInput;
    Vector2 moveDirection;
  

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCoolDown = 1f;
    bool isDashing;
    bool canDash;

    [Header("Animation")]
    [SerializeField] Animator _animator;
    private bool _LookRight;
    

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = new PlayerInput();
    }

    private void Start()
    {
        canDash = true;
    }
    private void Update()
    {
        if (isDashing) return;
        Tourner();
        moveDirection = new Vector2(_movementInput.x, _movementInput.y).normalized;
        
    }
    private void FixedUpdate()
    {
        if (isDashing) return;

        _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);

        _rigidbody.velocity = _smoothedMovementInput * _speed;
        _animator.SetFloat("Vertical", _movementInput.y);
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if(canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void OnEnable()
    {
        _playerInput.Player.Jump.Enable();
    }
    private void OnDisable()
    {
        _playerInput.Player.Jump.Disable();
    }

    void Tourner()
    {
        if ((_movementInput.x > 0 && _LookRight) || (_movementInput.x < 0 && !_LookRight))
        {
            _LookRight = !_LookRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            transform.Rotate(0f, 0f, 0f);
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _rigidbody.velocity = new Vector2(moveDirection.x * dashSpeed, moveDirection.y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}