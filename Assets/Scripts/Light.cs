using System.Collections;
using System.Collections.Generic;
using LightVolume;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Transform player;


    public float RotateSpeed = 1;
    public float WobbleAmplitude = 2;

    private float _angle;
    private float _targetAngle;

    private bool on => _flashlight.IsOn;
    
    private Light2D _flashlight;
    private FlickerLight _flickerLight;

    public float enemyStopRange = 5;
    
    void Start()
    {
	    transform.parent = null;
       _flashlight = GetComponentInChildren<Light2D>();
       _flickerLight = GetComponentInChildren<FlickerLight>();
       _flickerLight.enabled = false;
    }

    public bool CanSee(Transform t)
    {
	    Vector2 dir = t.position - transform.position;

	    if (dir.magnitude > enemyStopRange)
	    {
		    return false;
	    }

	    float range = Mathf.Min(dir.magnitude, enemyStopRange);

	    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, _flashlight.LightBlockMask);
	    bool canSee = hit.collider == null;

	    if (!canSee)
	    {
		    return false;
	    }

	    float angle = Vector2.Angle(transform.up, dir);
	    if (angle < _flashlight.angle * 0.5f)
	    {
		    return true;
	    }

	    return false;
    }
    
    void Update()
    {
	    
	    
	    if (Input.GetButtonDown("Light"))
	    {
		    _flashlight.SwitchLight(!on);
	    }

	    if (on)
	    {
		    if (Random.Range(0, _flickerLight.enabled ? 300 : 600) == 0)
		    {
			    _flickerLight.enabled = !_flickerLight.enabled;
		    }
	    }
	    
	    
	    transform.position = player.position;
	    _targetAngle = player.transform.eulerAngles.z + (Mathf.Sin(Time.time) + Mathf.Sin(Time.time*Mathf.PI) + Mathf.Sin(Time.time*5))*WobbleAmplitude;
        _angle = Mathf.LerpAngle(_angle, _targetAngle, Time.deltaTime * RotateSpeed);
        transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
