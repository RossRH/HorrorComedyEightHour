using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(PlayerResources))]

public class PlayerMovement : MonoBehaviour
{
    public float SprintForwardSpeed = 2;
    public float SprintBackwardSpeed = 2;
    public float MoveSpeedForward = 1.5f;
    public float MoveSpeedBackwards = 0.75f;
    public float RotateSpeed = 1;


    [Space] public float SprintSpeedUse = 1;
    

    private float stamina = 1;
    private float staminaRegenSpeed = 0;
    
    private Rigidbody2D _rigidbody;
    private PlayerResources _playerResources;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerResources = GetComponent<PlayerResources>();
    }

    private void Update()
    {
        bool sprint = Input.GetButton("Sprint");
        if (!sprint)
        {
            stamina += Time.deltaTime * staminaRegenSpeed;
        }
        else
        {
            _playerResources.UseStamina(SprintSpeedUse * Time.deltaTime);
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


        _rigidbody.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            SceneManager.GetActiveScene(); SceneManager.LoadScene(0);
        }
    }
}
