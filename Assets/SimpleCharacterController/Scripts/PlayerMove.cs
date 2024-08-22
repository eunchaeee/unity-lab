using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float gravityMultiplier = 3.0f;    // for fun game.
    [SerializeField] private float _jumpPower;
    [SerializeField] private int maxNumberOfJumps = 2;
   
    private Vector2 _input;
    private Vector3 _direction;
    private float _currentVelocity;
    private float _gravity = -9.81f;
    private float _velocity;
    private int _numberOfJumps;

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
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

        /*var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, rotationSpeed);
        transform.rotation = Quaternion.Euler(0, angle, 0);*/

        _direction = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0) * new Vector3(_input.x, 0, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        characterController.Move(_direction * speed * Time.deltaTime);
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
    private bool IsGrounded() => characterController.isGrounded;
}
