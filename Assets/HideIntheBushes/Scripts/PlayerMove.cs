using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed;

    private Vector2 _input;

    void Update()
    {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var dir = new Vector3(h, 0, v) * speed;
            cc.Move(dir * Time.deltaTime);  
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
        Debug.Log($"OnMove {_input}");
    }
}
