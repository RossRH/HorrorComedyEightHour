using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float SprintForwardSpeed = 2;
    public float SprintBackwardSpeed = 2;
    public float MoveSpeedForward = 1.5f;
    public float MoveSpeedBackwards = 0.75f;
    public float RotateSpeed = 1;

    private float stamina = 1;
    private float staminaRegenSpeed = 0;
    
    private Rigidbody2D _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool sprint = Input.GetButton("Sprint");
        if (!sprint)
        {
            stamina += Time.deltaTime * staminaRegenSpeed;
        }
        
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool sprint = Input.GetButton("Sprint");

        float moveSpeedMultiplier =  Mathf.Sign(vertical) > 0 ? (sprint ? SprintForwardSpeed : MoveSpeedForward) :  (sprint ? SprintBackwardSpeed : MoveSpeedBackwards);
        _rigidbody.MovePosition(_rigidbody.position + (Vector2)transform.up * vertical * Time.fixedDeltaTime * moveSpeedMultiplier);
        _rigidbody.MoveRotation(_rigidbody.rotation - horizontal * RotateSpeed);
    }
}
