using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;
    [SerializeField] Animator _animator;
    private bool _LookRight;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Tourner();
    }
    private void FixedUpdate()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(_smoothedMovementInput, _movementInput, ref _movementInputSmoothVelocity, 0.1f);

        _rigidbody.velocity = _smoothedMovementInput * _speed;
        _animator.SetFloat("Vertical", _movementInput.y);
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
    }
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    void Tourner()
    {
        if ((_movementInput.x > 0 && _LookRight) || (_movementInput.x < 0 && !_LookRight))
        {
            _LookRight = !_LookRight;
            transform.Rotate(0f, 180f, 0f);
            _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
        }
        else
        {
            transform.Rotate(0f, 0f, 0f);
        }
    }
}