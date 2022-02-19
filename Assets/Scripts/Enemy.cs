using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private Vector2 targetDir => ((Vector2)_player.position - (Vector2)transform.position);
    private Vector2 movementDir;
    private Rigidbody _rigidbody;
    public float moveSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDir = targetDir.normalized;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * moveSpeed * (Vector3)movementDir);
    }
}
