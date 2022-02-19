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
    private CircleCollider2D _collider;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
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
	    

	    bool IsBlocked(Vector2 p, Vector2 d)
	    {
		    return Physics2D.Raycast(p, d, 0.5f, _avoidanceLayerMask).collider != null;
	    }

	    bool IsBlockedFullCheck(Vector2 p, Vector2 d)
	    {
		    Vector2 ortho = new Vector2(vec.y, -vec.x) * _collider.radius * 1.05f;
		    
		    return IsBlocked(p + vec * _collider.radius, d) || IsBlocked(p + ortho, d) || IsBlocked(p - ortho, d);
	    }
	    
	    

	    while (IsBlockedFullCheck(transform.position, dir) && angleMagnitude <= 90)
	    {
		    dir = vec.Rotate(angleMagnitude);

		    if (IsBlockedFullCheck(transform.position, dir))
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
