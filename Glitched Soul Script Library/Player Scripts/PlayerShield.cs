using UnityEngine;
using System.Collections;

//Script Objective: The shield of the player when he blocks.

//NOTE THE SHIELD GUI WILL BE DISPLAYED ON THE PLAYERHEALTH SCRIPT TO RETAIN PERFECT POSITION

public class PlayerShield : MonoBehaviour 
{
	//Insert them variables over here bro:
	static public float playerShield;
	private float playerMinShield = 0.01f; //don't let the min be 0 of the cut off material will die.
	public float playerMaxShield = 100f;
	static public float shieldDisplay;

	//Shield Regeneration variables
	private float regenTimer;
	public float regenMaxTimer = 2f;
	public float regenSpeed = 4f; 


	// Use this for initialization
	void Start () 
	{
		//initiate shield variable
		playerShield = playerMaxShield;
		shieldDisplay = playerShield;

		//initiate regen timers
		regenTimer = regenMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Adjust the shield values to prevent negatives and/ or extra shield being calculated
		if(playerShield < playerMinShield) 		//If shield is less than minimum 
		{
			shieldDisplay = playerMinShield;	//the hud should display the minimum and not go into negatives
		}
		
		if(playerShield > playerMaxShield)
		{
			shieldDisplay = playerMaxShield;
			playerShield = playerMaxShield;
		}

		if(playerShield > playerMinShield && playerShield < playerMaxShield)
		{
			shieldDisplay = playerShield; 
		}

		//------------SHIELD REGENERATION-----------------
		//Constantly decrease time of the regenTime if less than max shield
		if(playerShield < playerMaxShield)
		{
			regenTimer -= Time.deltaTime;
		}

		//If the regenTimer hits 0 then the player shield will regenerate
		if(regenTimer <= 0f && WeaponUpgrade.charging == false)
		{
			playerShield += regenSpeed * Time.deltaTime;
		}
	}

	//This function is to deduct the amount of damage dealt to the shield
	public void applyShieldDamage(float _damage)
	{
		playerShield -= _damage;

		//Reset the regeneration time or delay, whatever
		regenTimer = regenMaxTimer;
	}
}
