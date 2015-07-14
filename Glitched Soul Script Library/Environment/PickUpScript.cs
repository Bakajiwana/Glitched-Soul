using UnityEngine;
using System.Collections;

//SCRIPT OBJECTIVE: When the player collects a green anomaly pick up collectible add points

public class PickUpScript : MonoBehaviour 
{
	public int pickUpPoints = 25;
	public float healthRestorePoints = 25f;

	public bool scorePickUp;
	public bool healthPickUp;

	//Weapon pick ups
	public bool shotgunPickUp;
	public bool laserPickUp;
	public bool chargePickUp;
	public bool grenadePickUp;
	public int grenadeAmount;

	//Power Up Pick Ups
	public bool powerUpPickUp;
	//Generate random number
	int randomAbility;	

	//particle effects
	public Transform collectParticle;
	public Transform healthParticle;
	public Transform weaponParticle;
	public Transform powerupParticle;
	public Transform grenadeParticle;

	void Start()
	{
		randomAbility = (Random.Range (1,4));
	}

	//When player collects green anomaly pick up
	void OnTriggerEnter(Collider other)
	{
		//if player picks up score pick up
		if(other.gameObject.CompareTag ("Player") && scorePickUp)
		{
			//Send points to player
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("applyScore", pickUpPoints);
			//Instantiate a particle effect
			Instantiate (collectParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}

		//if player picks up health pick up
		if(other.gameObject.CompareTag ("Player") && healthPickUp)
		{
			other.gameObject.SendMessage ("healthRestore", healthRestorePoints, SendMessageOptions.DontRequireReceiver); 
			//Instantiate a particle effect
			Instantiate (healthParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}

		//if player picks up shot gun pick up
		if(other.gameObject.CompareTag ("Player") && shotgunPickUp)
		{
			GameObject.FindGameObjectWithTag ("Player Weapon").SendMessage ("ShotgunPickup");
			//Instantiate a particle effect
			Instantiate (weaponParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}

		//If player picks up laser
		if(other.gameObject.CompareTag ("Player") && laserPickUp)
		{
			GameObject.FindGameObjectWithTag ("Player Weapon").SendMessage ("LaserPickup");
			//Instantiate a particle effect
			Instantiate (weaponParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}

		//if player picks up charge shot
		if(other.gameObject.CompareTag ("Player") && chargePickUp)
		{
			GameObject.FindGameObjectWithTag ("Player Weapon").SendMessage ("ChargeShotPickup");
			//Instantiate a particle effect
			Instantiate (weaponParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}


		//If Player picks up a grenade pick up
		if(other.gameObject.CompareTag ("Player") && grenadePickUp)
		{
			other.gameObject.SendMessage ("pickUpGrenade", grenadeAmount, SendMessageOptions.DontRequireReceiver);
			//Instantiate a particle effect
			Instantiate (grenadeParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}

		//if player picks up a power up pick up
		if(other.gameObject.CompareTag ("Player") && powerUpPickUp)
		{
			//Use a switch case to determine which ability should be used
			switch (randomAbility)
			{
			case 1: //Corrupt power up
				other.gameObject.SendMessage ("CorruptReady", SendMessageOptions.DontRequireReceiver);
				break;
			case 2: //Ward power up
				other.gameObject.SendMessage ("WardReady", SendMessageOptions.DontRequireReceiver);
				break;
			case 3: //Speed Power up
				other.gameObject.SendMessage ("SpeedReady", SendMessageOptions.DontRequireReceiver);
				break;
			}
			//Instantiate a particle effect
			Instantiate (powerupParticle, transform.position, transform.rotation);
			//Destroy self
			Destroy (gameObject);
		}
	}
}
