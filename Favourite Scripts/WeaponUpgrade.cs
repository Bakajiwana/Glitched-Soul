using UnityEngine;
using System.Collections;

//Script Objective to control the weapon upgrades the player obtains

/*Reference:
 * How to create a shotgun effect, http://forum.unity3d.com/threads/83775-Creating-a-shotgun
 * Shooting Lasers in Unity, http://blog.safaribooksonline.com/2013/11/20/shooting-lasers-in-unity/
*/

//In level 1 the player might obtain a shotgun upgrade for the gun
//In level 2 the player might obtain a laser upgrade
//In Level 3 the player might obtain a charge shot upgrade

public class WeaponUpgrade : MonoBehaviour 
{
	public StatScript stat; //Obtain variables from the stat script which is the level we are in

	//laser variables
	private int laser;
	LineRenderer line;
	public int laserDist = 100;
	static public bool stopBlast = true;
	public float laserDamage = 1f;
	public Transform laserMuzzleFlash;
	public Transform laserHit;
	public Transform enemyBlood;
	public float laserDrainRate = 20f;

	//Shotgun Variables
	private int shotgun;
	public Transform shotgunBullets;
	public float shotgunPelletSpeed = 150f;
	public int shotgunPelletCount = 5;
	public float shotgunSpreadFactor = 0.01f;
	public float shotgunFireRate = 0.5f;
	public Transform shotgunMuzzleFlash;
	public float shotgunEnergyDrain = 20f;

	//Charge Shot Variables
	private int chargeShot;
	private float charge;
	static public bool charging = false;
	public float chargeRate;
	public float chargeLevel01;
	public float chargeLevel02;
	public float chargeLevel03;
	public Transform chargeBullet01;
	public Transform chargeBullet02;
	public Transform chargeBullet03;
	public Transform bullet;
	public float chargeSpeed = 2000f;
	public Transform chargingLevel00;
	public Transform chargingLevel01;
	public Transform chargingLevel02;
	public Transform chargingLevel03;

	private int currentWeapon; //1 is Shotgun, 2 is Laser and 3 is Charge Shot
	//Using this method so I don't have to write a huge list of weapons to set booleans to false.

	private float nextFire = 0.0f; //Fire after the next

	//Bulletnode variable (where the projectiles will come out)
	public Transform bulletNode;

	//HUD variables
	public Transform shotgunSelection;
	public Transform shotgunActive;
	public Transform laserSelection;
	public Transform laserActive;
	public Transform chargeSelection;
	public Transform chargeActive;

	//----------SOUND CLIPS-----------
	public AudioClip chargeSound;
	public AudioClip laserSound;

	// Use this for initialization
	void Start () 
	{
		//If in testing room
		if(stat.level == 0)
		{
			PlayerPrefs.SetInt ("Shotgun" , 1);
			PlayerPrefs.SetInt ("Laser" , 1);
			PlayerPrefs.SetInt ("ChargeShot" , 1);

			shotgun = PlayerPrefs.GetInt ("Shotgun", 1);	
			laser = PlayerPrefs.GetInt ("Laser", 1);			
			chargeShot = PlayerPrefs.GetInt ("ChargeShot", 1);


		}
		//In level 1 every weapon should be disabled because the player hasn't picked up any upgrade yet
		if(stat.level == 1)
		{
			PlayerPrefs.SetInt ("Shotgun" , 0);
			PlayerPrefs.SetInt ("Laser" , 0);
			PlayerPrefs.SetInt ("ChargeShot" , 0);

			shotgun = PlayerPrefs.GetInt ("Shotgun", 0);		//Set the shotgun variable to 0 which means false
			laser = PlayerPrefs.GetInt ("Laser", 0);			//Set the laser variable to 0 which means false
			chargeShot = PlayerPrefs.GetInt ("ChargeShot", 0);	//Set the charge shot variable to 0 which means false


		}

		//In Level 2 every weapon except shotgun should still be false (0) because in level 1 the player may have picked up a shotgun
		if(stat.level == 2)
		{
			PlayerPrefs.SetInt ("Laser" , 0);
			PlayerPrefs.SetInt ("ChargeShot" , 0);

			shotgun = PlayerPrefs.GetInt ("Shotgun");
			laser = PlayerPrefs.GetInt ("Laser" , 0);
			chargeShot = PlayerPrefs.GetInt ("ChargeShot", 0);


		}

		//In Level 3 every weapon except charge shot should have been picked up in previous levels, or not. That just means we don't set the player prefs to 0
		if(stat.level == 3)
		{
			PlayerPrefs.SetInt ("ChargeShot" , 0);

			shotgun = PlayerPrefs.GetInt ("Shotgun");
			laser = PlayerPrefs.GetInt ("Laser");
			chargeShot = PlayerPrefs.GetInt ("ChargeShot", 0);


		}


		//Create a line renderer component for the laser
		line = gameObject.GetComponent<LineRenderer>();
		line.enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If 1 is pressed and the shotgun player prefs integer is set to 1 then the shotgun can be activated
		if(shotgun == 1 && Input.GetKeyDown (KeyCode.Alpha1))
		{
			currentWeapon = 1;
		}

		//If 1 is pressed and the laser player prefs integer is set to 1 then the laser can be activated
		if(laser == 1 && Input.GetKeyDown (KeyCode.Alpha2))
		{
			currentWeapon = 2;
		}

		//If 1 is pressed and the Charge shot player prefs integer is set to 1 then the charge shot can be activated
		if(chargeShot == 1 && Input.GetKeyDown (KeyCode.Alpha3))
		{
			currentWeapon = 3;
		}
		
		//HUD DISPLAY ACTIVE OR NOT ACTIVE
		if(shotgun == 1)
		{
			shotgunActive.gameObject.SetActive (false);		//reveal shotgun
		}
		else
		{
			shotgunActive.gameObject.SetActive (true);		//grey out shotgun
		}

		if(laser == 1)
		{
			laserActive.gameObject.SetActive (false);		//reveal laser
		}
		else
		{
			laserActive.gameObject.SetActive (true);		//grey out laser
		}

		if(chargeShot == 1)
		{
			chargeActive.gameObject.SetActive (false);		//reveal charge shot
		}
		else 
		{
			chargeActive.gameObject.SetActive (true);		//grey out charge shot
		}

		//----------------------------------WHEN WEAPON IS SELECTED-----------------------------------
		//IF SHOTGUN IS ACTIVATED
		if(currentWeapon == 1)
		{
			//Shotgun Selection
			shotgunSelection.gameObject.SetActive (true);	//Shotgun is selected as shown in HUD
			laserSelection.gameObject.SetActive (false);	//laser is not selected
			chargeSelection.gameObject.SetActive (false);	//Charge Shot is not selected 

			charging = false; //Allow energy regeneration
			charge = 0f; //reset the charge, for the charge shot
			//If the right mouse button is clicked
			if(Input.GetButtonDown ("Fire2") && Time.time > nextFire && PlayerShield.playerShield >= shotgunEnergyDrain)
			{
				//For how many shot gun pellets specified
				for (int i = 0; i < shotgunPelletCount; i++)
				{
					foreach(Transform child in bulletNode)
					{
						Quaternion pelletRot = transform.rotation;
						pelletRot.x += Random.Range (-shotgunSpreadFactor, shotgunSpreadFactor);
						pelletRot.y += Random.Range (-shotgunSpreadFactor, shotgunSpreadFactor);
						Transform pellet = Instantiate (shotgunBullets, child.position, pelletRot) as Transform;
						pellet.rigidbody.AddForce (transform.right * shotgunPelletSpeed);
					}
				}
				nextFire = Time.time + shotgunFireRate; //shotgun fire rate

				PlayerShield.playerShield -= shotgunEnergyDrain; //Drain energy 

				stopBlast = false;	//allow the blast animations when fired

				foreach(Transform child in bulletNode)
				{
					Transform shotgunMuzzle = Instantiate (shotgunMuzzleFlash, child.position, child.rotation) as Transform;
					shotgunMuzzle.transform.parent = transform;
				}
			}
		}

		//IF LASER IS ACTIVATED
		if(currentWeapon == 2)
		{
			//Laser Selection
			shotgunSelection.gameObject.SetActive (false);	//Shotgun is not selected
			laserSelection.gameObject.SetActive (true);	//laser is selected
			chargeSelection.gameObject.SetActive (false);	//Charge Shot is not selected 

			//If laser is activated make sure the blast animation is never played
			stopBlast = true;					//Stop the blast animation when fired because lazer.
			charge = 0f; 						//reset the charge, for the charge shot
			if(Input.GetButtonDown ("Fire2"))	//We only need to have the button down once to start co routine and not use GetButton.
			{
				StopCoroutine("FireLaser");
				StartCoroutine("FireLaser");	//Fire the lasers
				laserMuzzleFlash.gameObject.SetActive (true);	//Show the laser muzzle flash

				audio.Stop ();
				audio.PlayOneShot (laserSound, 0.5f);

			}
			if(Input.GetButtonUp ("Fire2") || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.LeftAlt))
			{
				audio.Stop ();
				laserMuzzleFlash.gameObject.SetActive (false);
				charging = false;
			}
		}

		//IF CHARGE SHOT IS ACTIVATED
		if(currentWeapon == 3)
		{
			//Charge shot Selection
			shotgunSelection.gameObject.SetActive (false);	//Shotgun is not selected
			laserSelection.gameObject.SetActive (false);	//Laser is not selected
			chargeSelection.gameObject.SetActive (true);	//Charge Shot is selected

			stopBlast = false;	//allow the blast animations when fired
			//While the player is holding the fire button, charge up shot
			if(Input.GetButton ("Fire2"))
			{
				//Charge up shot and decrease energy 
				if(PlayerShield.playerShield >= 0f && charge <= chargeLevel03)
				{
					charge += Time.deltaTime * chargeRate;
					PlayerShield.playerShield -= Time.deltaTime * chargeRate;
					charging = true;
				}
			}

			if(Input.GetButtonDown ("Fire2"))
			{
				audio.Stop ();
				audio.PlayOneShot (chargeSound, 0.5f);
			}
			if (Input.GetButtonUp ("Fire2") || Input.GetKeyDown (KeyCode.F) || Input.GetKeyDown (KeyCode.LeftAlt))
			{
				charging = false;
				audio.Stop ();
				//Loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in bulletNode)
				{
					if(charge >= chargeLevel03)
					{
						//Shoot bullet
						Transform chargeShoot = Instantiate (chargeBullet03, child.position, child.rotation) as Transform;
						chargeShoot.rigidbody.AddForce (transform.right * chargeSpeed);
					}
					else if (charge >= chargeLevel02)
					{
						//Shoot bullet
						Transform chargeShoot = Instantiate (chargeBullet02, child.position, child.rotation) as Transform;
						chargeShoot.rigidbody.AddForce (transform.right * chargeSpeed);
					}
					else if (charge >= chargeLevel01)
					{
						//Shoot bullet
						Transform chargeShoot = Instantiate (chargeBullet01, child.position, child.rotation) as Transform;
						chargeShoot.rigidbody.AddForce (transform.right * chargeSpeed);
					}
					else if (charge >= 0f)
					{
						//Shoot bullet
						Transform chargeShoot = Instantiate (bullet, child.position, child.rotation) as Transform;
						chargeShoot.rigidbody.AddForce (transform.right * chargeSpeed);
					}
					charge = 0f;
				}
			}
		}

		//WHEN CHARGING THE CHARGING PARTICLES SHOULD APPEAR
		if(charge >= chargeLevel03)	//When in charge level 3 show the charge level 3 prefab and so on....
		{
			chargingLevel03.gameObject.SetActive (true); 
			chargingLevel02.gameObject.SetActive (false); 
			chargingLevel01.gameObject.SetActive (false); 
			chargingLevel00.gameObject.SetActive (false); 
		}
		else if(charge >= chargeLevel02)
		{
			chargingLevel03.gameObject.SetActive (false); 
			chargingLevel02.gameObject.SetActive (true); 
			chargingLevel01.gameObject.SetActive (false); 
			chargingLevel00.gameObject.SetActive (false); 
		}
		else if(charge >= chargeLevel01)
		{
			chargingLevel03.gameObject.SetActive (false); 
			chargingLevel02.gameObject.SetActive (false); 
			chargingLevel01.gameObject.SetActive (true); 
			chargingLevel00.gameObject.SetActive (false); 
		}
		else if(charge > 0f)
		{
			chargingLevel03.gameObject.SetActive (false); 
			chargingLevel02.gameObject.SetActive (false); 
			chargingLevel01.gameObject.SetActive (false); 
			chargingLevel00.gameObject.SetActive (true); 
		}
		else if(charge == 0f)
		{
			chargingLevel03.gameObject.SetActive (false); 
			chargingLevel02.gameObject.SetActive (false); 
			chargingLevel01.gameObject.SetActive (false); 
			chargingLevel00.gameObject.SetActive (false); 
		}
	}

	//This function is called when the shotgun is picked up
	public void ShotgunPickup()
	{
		PlayerPrefs.SetInt ("Shotgun" , 1);
		shotgun = PlayerPrefs.GetInt ("Shotgun", 1);	//For the shotgun integer to be 1, means it is true and can now be used.

	}

	//This function is called when the laser is picked up
	public void LaserPickup()
	{
		PlayerPrefs.SetInt ("Laser" , 1);
		laser = PlayerPrefs.GetInt ("Laser", 1);

	}

	//This function is called when the Charge Shot is picked up 
	public void ChargeShotPickup()
	{
		PlayerPrefs.SetInt ("ChargeShot" , 1);
		chargeShot = PlayerPrefs.GetInt ("ChargeShot", 1);

	}

	//This enumerator is used to fire the laser
	IEnumerator FireLaser()
	{
		//Enable the line renderer
		line.enabled = true;

		//While holding down the right mouse button and make sure the melee button is pressed
		while(Input.GetButton("Fire2") && !Input.GetKeyDown (KeyCode.F) && !Input.GetKeyDown (KeyCode.Alpha1) && !Input.GetKeyDown (KeyCode.Alpha3) && PlayerShield.playerShield >= 0f)
		{
			PlayerShield.playerShield -= Time.deltaTime * laserDrainRate;
			charging = true;

			//Create a raycast to go from secondary node position
			Ray ray = new Ray(bulletNode.position, transform.right);
			RaycastHit hit;

			//set the line renderer position to be at the end of the gun
			line.SetPosition(0, ray.origin);

			//Emit ray cast
			if(Physics.Raycast(ray, out hit, laserDist))
			{
				line.SetPosition(1, hit.point);
				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.right * 10, hit.point);

					if(hit.transform.CompareTag ("Enemy")) //enemy hit
					{				
						Instantiate (enemyBlood, hit.point, transform.rotation); //make it bleed
						Instantiate (laserHit, hit.point, transform.rotation);
						hit.transform.SendMessage ("HitByPlayer", SendMessageOptions.DontRequireReceiver);
						hit.transform.SendMessage ("applyEnemyDamage", laserDamage, SendMessageOptions.DontRequireReceiver); // and consume its health	
					}

					if(hit.transform.CompareTag ("Enemy Missile"))
					{
						hit.transform.SendMessage ("Destroyed", SendMessageOptions.DontRequireReceiver); // and consume its health
					}

					if(hit.transform.CompareTag ("Breakable"))
					{
						Instantiate (laserHit, hit.point, transform.rotation);
						hit.transform.SendMessage ("applyObjectDamage", laserDamage, SendMessageOptions.DontRequireReceiver); // and consume its health
					}
				}

				if(hit.transform.CompareTag ("Environment"))
				{
					//otherwise emit sparks at the hit point
					Instantiate (laserHit, hit.point, transform.rotation);
				}
			}
			else
			{
				line.SetPosition(1, ray.GetPoint(laserDist));
			}			
			yield return null;
		}
		laserMuzzleFlash.gameObject.SetActive (false);
		line.enabled = false;
	}
}
