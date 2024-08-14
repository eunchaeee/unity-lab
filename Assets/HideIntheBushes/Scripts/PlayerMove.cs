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

    void Update()
    {
        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
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
}
