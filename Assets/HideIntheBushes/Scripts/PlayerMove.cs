using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed;
    [SerializeField] private float smoothTime = 0.05f;

    
    private Vector2 _input;
    private Vector3 _direction;
    private float _currentVelocity;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;    // for fun game.
    private float _velocity;

    [SerializeField] private float _jumpPower;

    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity <0f)
        {
            _velocity = 0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.magnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void ApplyMovement()
    {
        cc.Move(_direction * speed * Time.deltaTime);
    }

    // For SendMessages => But has dependancy to C# reflection.
    /*
    public void OnMove(InputValue value)
    {
        _input = value.Get<Vector2>();
        Debug.Log($"OnMove {_input}");
    }
    */

    public void OnMove(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0, _input.y);
        Debug.Log($"OnMove {_input}");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());
        
        _numberOfJumps++;
        _velocity = _jumpPower/_numberOfJumps;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());                                    
        yield return new WaitUntil(IsGrounded);
        _numberOfJumps = 0;
    }
    private bool IsGrounded() => cc.isGrounded;
}
