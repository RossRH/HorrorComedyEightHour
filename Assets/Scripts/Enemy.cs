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

        float increments = 5;
        float angleMagnitude = increments;
        Vector2 targetNormalised = targetDir.normalized;

        Vector2 dir = targetNormalised;

        while (Physics.Raycast(transform.position, dir, 100000, _avoidanceLayerMask) && angleMagnitude <= 90)
        {
            dir = targetNormalised.Rotate(angleMagnitude);

            if (Physics.Raycast(transform.position, dir, 100000, _avoidanceLayerMask))
            {
                dir = targetNormalised.Rotate(-angleMagnitude);
            }
            else
            {
                break;
            }

            angleMagnitude += increments;
        }

        movementDir = dir;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * moveSpeed * movementDir);
    }
}
