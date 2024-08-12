using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed;

    void Update()
    {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            var dir = new Vector3(h, 0, v) * speed;
            cc.Move(dir * Time.deltaTime);  
    }
}
