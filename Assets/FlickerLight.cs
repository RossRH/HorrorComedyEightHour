using System;
using System.Collections;
using System.Collections.Generic;
using LightVolume;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light2D))]
public class FlickerLight : MonoBehaviour
{

    private Light2D _light;

    private float _currentIntensity, _lastIntensity;
    private float flickerTime = 0;
    public float flickerInterval = 0.25f;
    public float minIntensity, maxIntensity;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();
        _currentIntensity = _light.Intensity;
        _lastIntensity = _currentIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        
        flickerTime += Time.deltaTime;

        if (flickerTime >= flickerInterval) {
            _lastIntensity = _currentIntensity;
            _currentIntensity = Random.Range(minIntensity, maxIntensity);
            flickerTime = 0;
        }
        
        _light.Intensity = Mathf.Lerp (_lastIntensity, _currentIntensity, flickerTime / flickerInterval);
    }


    private void OnDisable()
    {
        _light.Intensity = _currentIntensity;
    }


    // Update is called once per frame
}
