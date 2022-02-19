using System;
using System.Collections;
using System.Collections.Generic;
using GraveDays.Utility;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private Vector3 targetDir => _player.position - transform.position;
    private Rigidbody2D _rigidbody;
    public float moveSpeed;

    [SerializeField] private LayerMask _avoidanceLayerMask;

    private Vector2 lastKnownPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
	    _rigidbody.velocity = ApplyCollisionAvoidance(GetMovementVector()).normalized * moveSpeed;
    }

    private Vector2 ApplyCollisionAvoidance(Vector2 vec)
    {
	    vec = vec.normalized;
	    Vector2 dir = vec;
        
        
	    float increments = 5;
	    float angleMagnitude = increments;
	    
	    

	    while (Physics2D.Raycast(transform.position, dir, 0.1f, _avoidanceLayerMask).collider != null && angleMagnitude <= 90)
	    {
		    dir = vec.Rotate(angleMagnitude);

		    if (Physics2D.Raycast(transform.position, dir, 0.1f, _avoidanceLayerMask).collider != null)
		    {
			    dir = vec.Rotate(-angleMagnitude);
		    }
		    else
		    {
			    break;
		    }

		    angleMagnitude += increments;
	    }

	    return dir;
    }

    private Vector2 GetMovementVector()
    {
	    RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, targetDir.magnitude, _avoidanceLayerMask);
	    bool canSee = hit.collider == null;
	    
	    
	    if (!canSee)
	    {
		    return (lastKnownPosition - (Vector2)transform.position);
	    }

	    lastKnownPosition = _player.transform.position;

	    return targetDir;
    }

    private void FixedUpdate()
    {
       // _rigidbody.MovePosition(_rigidbody.position + Time.fixedDeltaTime * moveSpeed * movementDir);
       
    }
}
