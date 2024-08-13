using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed;
    
    private Vector2 _input;
    private Vector3 _direction;

    void Update()
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
}
