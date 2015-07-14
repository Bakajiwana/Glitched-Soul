using UnityEngine;
using System.Collections;

//Script Objective: Control the health of this enemy and anything that affects its well being lel.

public class CyborgGunnerHealth : MonoBehaviour {

	//Health variables
	private float enemyHealth; //health of enemy

	public float enemyMaxHealth = 100f;

	private bool lastHitByPlayer = false; //boolean to give player points if the last hit was from player. Just in case death is not by player

	public Transform ragdoll; //ragdoll transform

	public GameObject self; //Whole object itself (including its own waypoints etc)

	public int points = 5; //points applied to player when enemy killed

	public CyborgGunnerScript cyborgScript; //script from enemy cyborg

	public Transform player;	//Transform of player

	public float stealthHitMultiplier = 3f;

	private Vector3 initialPosition;	//Remember the initial position of this enemy

	//Stagger Variables
	public float staggerMaxTimer = 1f;
	private float staggerTimer = 0f;

	[System.NonSerialized]	// Don't want to see the stagger bool in inspector
	public bool stagger;


	void Start()
	{
		enemyHealth = enemyMaxHealth;
		cyborgScript.lineOfSightTimer = 0f;
		initialPosition = transform.position; 
	}

	// Update is called once per frame
	void Update () 
	{
		//If enemy health hits 0 then die
		if(enemyHealth <= 0f)
		{
			Death();
		}

		//If enemy is staggered let the countdown for stagger begin
		//Script Objective: When enemy is hit, decrease his speed (going to have to communicate with cyborg destination script)
		//Make sure stagger is counting down
		if(stagger)
		{
			staggerTimer -= Time.deltaTime;
		}

		//If the stagger timer is less or equal to 0 then stagger is false
		if(staggerTimer <= 0f)
		{
			stagger = false;
		}
	}

	public void Death()
	{
		//if the last damage was done by the player then, count as death and apply score
		if(lastHitByPlayer == true)
		{
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("applyKill");			//Apply kill stat
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("applyKillScore", points);	//Apply score
			transform.gameObject.SendMessage ("pickUp", SendMessageOptions.DontRequireReceiver);	//Drop item
		}
		enemyHealth = enemyMaxHealth;
		cyborgScript.lineOfSightTimer = 0f;
		cyborgScript.detected = false;	
		cyborgScript.elite = false;
		//Instantiate ragdoll
		Instantiate (ragdoll, transform.position, transform.rotation);
		//Return to initial position so it can be respawned if player restarts checkpoint
		transform.position = initialPosition;
		//Destroy self
		self.gameObject.SetActive (false);
	}

	//This function is called to take damage
	public void applyEnemyDamage (float _damage)
	{
		//If not detected then take 3x damage
		if(cyborgScript.detected == false)
		{
			//Decrease health
			enemyHealth -= _damage * stealthHitMultiplier; //take damage with multiplier because not paying attention.
		}
		else
		{
			enemyHealth -= _damage; //take damage
		}

		cyborgScript.detected = true;						//Player is now detected
		cyborgScript.hit = true;							//Enemy will do a hit animation
		//Reset the lineOfSightTimer
		cyborgScript.lineOfSightTimer = cyborgScript.lineOfSightMaxTimer;

		//Initiate stagger function
		Stagger();

		//Notify Music to turn into combat
		GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
	}

	//This function is called to stagger the enemy when damage is taken
	void Stagger()
	{
		//reset stagger timer
		staggerTimer = staggerMaxTimer;
		//Stagger is now true
		stagger = true;
	}

	public void HitByPlayer()
	{
		lastHitByPlayer = true;
	}

	public void NaturalDeath()
	{
		lastHitByPlayer = false;
	}

	void OnCollisionEnter(Collision other)
	{
		//If hit by player projectile, or bullet...
		if(other.gameObject.CompareTag ("Player Bullet") || other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Player Weapon"))
		{
			//The last hit by player is now true and this enemy can die and add to the kill count and score
			lastHitByPlayer = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//If hit by player projectile, or bullet...
		if(other.gameObject.CompareTag ("Player Bullet") || other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Player Weapon"))
		{
			//The last hit by player is now true and this enemy can die and add to the kill count and score
			lastHitByPlayer = true;
		}
	}
}
