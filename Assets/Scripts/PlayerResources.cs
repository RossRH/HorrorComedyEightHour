using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
	public float MaxHealth = 5;
	public float MaxStamina = 100;

	private float _discreteHealth;
	private float _health;
	private float _stamina;

	private float lastDamageTime;
	private float lastStaminaUseTime;
	
	public float HealthRegenSpeed;
	public float StaminaRegenSpeed;

	public float TimeAfterDamageHealingCanStart = 5;
	public float TimeAfterStaminaUseRegenCanStart = 2;
	
	public Shapes.Rectangle StaminaRectangle;
	public List<GameObject> Hearts;

	private float fullRectangle = 4;
	
    void Start()
    {
	    _health = MaxHealth;
	    _stamina = MaxStamina;
	    lastDamageTime = 0;
	    lastStaminaUseTime = 0;
    }

    public void UseStamina(float used)
    {
	    _stamina -= used;
	    lastStaminaUseTime = Time.time;
    }

    public void Damaged(float damage)
    {
	    _health -= damage;
    }
    
    void Update()
    {
	    if (Time.time - lastDamageTime > TimeAfterDamageHealingCanStart)
	    {
		    _health = Mathf.Min(_health + HealthRegenSpeed * Time.deltaTime, MaxHealth);
	    }
	    
	    if (Time.time - lastStaminaUseTime > TimeAfterStaminaUseRegenCanStart)
	    {
		    _stamina = Mathf.Min(_stamina + StaminaRegenSpeed * Time.deltaTime, MaxStamina);
	    }
	    
	    _discreteHealth = Mathf.Ceil(_health);

	    StaminaRectangle.Width = (_stamina / MaxStamina)*4;

	    for(int i = 0; i < Hearts.Count && i < MaxHealth; i++)
	    {
		    if (i < _discreteHealth)
		    {
			    Hearts[i].SetActive(true);
		    }
		    else
		    {
			    Hearts[i].SetActive(false);
		    }
	    }
    }
}
