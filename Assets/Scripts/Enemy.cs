using System;
using System.Collections;
using System.Collections.Generic;
using GraveDays.Utility;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private Vector3 targetDir => _player.position - transform.position;
    private Vector2 movementDir;
    private Rigidbody2D _rigidbody;
    public float moveSpeed;

    [SerializeField] private LayerMask _avoidanceLayerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDir = targetDir.normalized;

       /* float increments = 5;
        float angleMagnitude = increments;
        Vector2 targetNormalised = targetDir;

        Vector2 dir = targetNormalised;

        while (Physics2D.Raycast(transform.position, dir, targetDir.magnitude, _avoidanceLayerMask) && angleMagnitude <= 90)
        {
            dir = targetNormalised.Rotate(angleMagnitude);

            if (Physics2D.Raycast(transform.position, dir, targetDir.magnitude, _avoidanceLayerMask))
            {
                dir = targetNormalised.Rotate(-angleMagnitude);
            }
            else
            {
                break;
            }

            angleMagnitude += increments;
        }

        movementDir = dir;*/
        _rigidbody.velocity = movementDir * moveSpeed;
    }

    private void FixedUpdate()
    {
       // _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * moveSpeed * movementDir);
       
    }
}
