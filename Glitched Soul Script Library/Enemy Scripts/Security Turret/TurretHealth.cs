using UnityEngine;
using System.Collections;

public class TurretHealth : MonoBehaviour 
{

	//Health variables
	private float enemyHealth; //health of enemy

	public float enemyMaxHealth;
	
	private bool lastHitByPlayer = false; //boolean to give player points if the last hit was from player. Just in case death is not by player
	
	public Transform deathParticles;
	
	public GameObject self; //Whole object itself (including its own waypoints etc)
	
	public int points = 5; //points applied to player when enemy killed
	
	public Transform player;	//Transform of player
	
	public float stealthHitMultiplier = 3f;
	
	public TurretScript droneShootScript;	


	void Start()
	{
		enemyHealth = enemyMaxHealth;
	}


	// Update is called once per frame
	void Update () 
	{
		//If enemy health hits 0 then die
		if(enemyHealth <= 0f)
		{
			Death();
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
		droneShootScript.gunLos = false;					
		droneShootScript.gunLosTimer = 0f;
		//Instantiate ragdoll
		Instantiate (deathParticles, transform.position, transform.rotation);
		//Destroy self
		self.gameObject.SetActive (false);
	}
	
	//This function is called to take damage
	public void applyEnemyDamage (float _damage)
	{
		//If not detected then take 3x damage
		if(droneShootScript.gunLos == false)
		{
			//Decrease health
			enemyHealth -= _damage * stealthHitMultiplier; //take damage with multiplier because not paying attention.
		}
		else
		{
			enemyHealth -= _damage; //take damage
		}
		
		droneShootScript.gunLos = true;						//Player is now detected
		//Reset the lineOfSightTimer
		droneShootScript.gunLosTimer = droneShootScript.gunMaxLos;

		//Notify Music to turn into combat
		GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
	}

	public void alertOthers()
	{
		droneShootScript.gunLos = true;						//Player is now detected
		//Reset the lineOfSightTimer
		droneShootScript.gunLosTimer = droneShootScript.gunMaxLos;
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
