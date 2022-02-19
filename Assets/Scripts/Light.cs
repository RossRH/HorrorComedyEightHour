using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Transform player;


    public float RotateSpeed = 1;
    public float WobbleAmplitude = 2;

    private float _angle;
    private float _targetAngle;

    private bool on;
    
    void Start()
    {
	    on = true;
       transform.parent = null;
    }
    
    void Update()
    {
	    if (Input.GetButtonDown("Light"))
	    {
		    on = !on;
		    transform.GetChild(0).gameObject.SetActive(on);
	    }
	    
	    
	    transform.position = player.position;
	    _targetAngle = player.transform.eulerAngles.z + (Mathf.Sin(Time.time) + Mathf.Sin(Time.time*Mathf.PI) + Mathf.Sin(Time.time*5))*WobbleAmplitude;
        _angle = Mathf.LerpAngle(_angle, _targetAngle, Time.deltaTime * RotateSpeed);
        transform.rotation = Quaternion.Euler(0, 0, _angle);
    }
}
