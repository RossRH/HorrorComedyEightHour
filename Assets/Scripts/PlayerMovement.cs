using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float SprintForwardSpeed = 2;
    public float SprintBackwardSpeed = 2;
    public float MoveSpeedForward = 1.5f;
    public float MoveSpeedBackwards = 0.75f;
    public float RotateSpeed = 1;
    
    private Rigidbody _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool sprint = Input.GetButton("Sprint");

        float moveSpeedMultiplier =  Mathf.Sign(vertical) > 0 ? (sprint ? SprintForwardSpeed : MoveSpeedForward) :  (sprint ? SprintBackwardSpeed : MoveSpeedBackwards);
        _rigidbody.MovePosition(_rigidbody.position + transform.forward * vertical * Time.fixedDeltaTime * moveSpeedMultiplier);
        _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.AngleAxis(horizontal * RotateSpeed , Vector3.up));
    }
}
