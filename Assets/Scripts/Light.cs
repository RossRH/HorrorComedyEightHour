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
    
    void Start()
    {
	    transform.parent = null;
       _flashlight = GetComponentInChildren<Light2D>();
       _flickerLight = GetComponentInChildren<FlickerLight>();
       _flickerLight.enabled = false;
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
