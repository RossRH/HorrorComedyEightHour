using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
	public float MaxHealth = 5;
	public float MaxStamina = 100;

	private float _health;
	private float _stamina;

	public float lastDamageTime;
	public float lastStaminaUseTime;
	
	public float HealthRegenSpeed;
	public float StaminaRegenSpeed;

	public float TimeAfterDamageHealingCanStart = 5;
	public float TimeAfterStaminaUseRegenCanStart = 2;
	
    void Start()
    {
	    _health = MaxHealth;
	    _stamina = MaxStamina;
	    lastDamageTime = float.MinValue;
	    lastStaminaUseTime = float.MinValue;
    }
    
    void Update()
    {
	    if (Time.time - lastDamageTime > TimeAfterDamageHealingCanStart)
	    {
		    _health = Mathf.Min(_health + HealthRegenSpeed * Time.deltaTime, MaxHealth);
	    }
	    
	    if (Time.time - lastStaminaUseTime > TimeAfterStaminaUseRegenCanStart)
	    {
		    _stamina = Mathf.Min(_health + StaminaRegenSpeed * Time.deltaTime, MaxStamina);
	    }
    }
}
