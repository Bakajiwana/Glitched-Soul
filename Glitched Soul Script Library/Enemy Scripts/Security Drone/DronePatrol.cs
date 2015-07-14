using UnityEngine;
using System.Collections;
//Script Objective: Get the Drone to patrol between any number of points while looking for the player

public class DronePatrol : MonoBehaviour 
{
	//Patrol, Alert, Attack phases
	public Transform player; 

	//Patrol Variables
	public float moveSpeed = 5f;
	public float moveRotate = 5f;

	[System.NonSerialized]	// Don't want to see in inspector
	public bool detected;
	public float sightDistance = 30f;
	public float sightRadius = 15f;

	//Destination variables
	public Transform[] wayPoint; 
	private int currentWayPoint = 0;

	public float speed = 3f;

	//Rotation variables
	public float damping = 5f;

	//Idle Variables
	private bool idle;
	public float idleMaxTimer = 2f;
	private float idleTimer;


	//Line of sight variables
	[System.NonSerialized]	// Don't want to see in inspector
	public float losTimer;
	public float losMaxTimer = 5f;

	//Attack variables
	[System.NonSerialized]	// Don't want to see in inspector
	public bool attack;

	public DroneShoot droneShootScript;

	//Chase variables
	public float closeDist;
	public float chaseDist;
	public float closeSpeed;
	public float chaseSpeed;

	//When player is on head, there should be a get the fuck off effect.
	public float radius = 200.0F;
	public float power = 1500.0F;

	//Alert variable 
	public float alertRadius; 

	//Corruption variables - when player is corrupting enemy
	public float corruptDist = 20f;
	public Transform corruptParticle;	

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//LINE OF SIGHT
		//Use a spherecast to scan area
		RaycastHit hit;
		if(Physics.SphereCast (transform.position, sightRadius, -transform.up, out hit, sightDistance))
		{
			//If drone comes across player then he is detected
			if(hit.transform.CompareTag ("Player"))
			{
				detected = true; 	//The player
				losTimer = losMaxTimer;	//Reset line of sight tiemr
				//Notify Music to turn into combat
				GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
			}
		}

		//If detected is true and not corrupted by player powerup
		if(PlayerUpgrade.corrupt == false)
		{
			//Disable corrupt particle
			corruptParticle.gameObject.SetActive (false);

			if(detected)
			{
				if(player)
				{
					attack = true;
					losTimer -= Time.deltaTime;


					float playerDist = Vector3.Distance (player.position, transform.position);

					if(playerDist >= chaseDist)
					{
						transform.Translate(chaseSpeed * Vector3.forward * Time.deltaTime);

						//Aim on the side the player is in
						Vector3 lookPos = player.position - transform.position;
						lookPos.y = 0;
						
						Quaternion rotation = Quaternion.LookRotation (lookPos);
						transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
					}
					else if (playerDist >= closeDist)
					{
						transform.Translate(closeSpeed * Vector3.forward * Time.deltaTime);
					}


					//ALERT OTHERS NEARBY
					//Return all colliders into an array within the OverlapSphere radius
					Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius);
					
					
					//For each collider inside the OverlapSphere, get pushed back by the AddExplosionForce
					foreach (Collider enemy in colliders) 
					{
						if (!enemy) 
						{
							continue;
						}
						
						if(enemy.transform.CompareTag("Enemy"))
						{
							enemy.gameObject.SendMessage ("alertOthers", SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
			else //If not detected then patrol the area
			{
				if(!idle) //if not idle then move to next point
				{
					float step = speed * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, wayPoint[currentWayPoint].position, step); 

					//rotate towards the waypoint
					Vector3 lookPos = wayPoint[currentWayPoint].position - transform.position;
					lookPos.y = 0;
					
					Quaternion rotation = Quaternion.LookRotation (lookPos);
					transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
				}
				else //pause for a bit 
				{
					if(idleTimer >= 0f)
					{
						idle = true;
						idleTimer -= Time.deltaTime;
					}
					else
					{
						idle = false;
					}
				}
			}


			//If losTimer runs out, player is no longer detected
			if(losTimer <= 0f && !droneShootScript.gunLos)
			{
				detected = false;
				attack = false;
			}
		}

		if(player && PlayerUpgrade.corrupt == true)
		{
			//calculate distance of player
			float playerDist = Vector3.Distance(player.position, transform.position);
			
			if(playerDist <= corruptDist)
			{
				corruptParticle.gameObject.SetActive (true);
			}
		}
	}


	//This function is called when the destination cube needs to switch to a new waypoint.
	void SwitchWaypoint()
	{
		//Increment the current way point so the array of the wayPoint variable will move up
		if(currentWayPoint < wayPoint.Length)
		{
			currentWayPoint ++;
		}
		
		if(currentWayPoint == wayPoint.Length)
		{
			currentWayPoint = 0;
		}
	}


	void getOffMe()
	{
		//Tweak this code by using Rigidbody.AddExplosionForce to blow the enemies away
		//and also hurts the player 
		Vector3 annoyingPos = transform.position;
		
		//Return all colliders into an array within the OverlapSphere radius
		Collider[] colliders = Physics.OverlapSphere(annoyingPos, radius);
		
		
		//For each collider inside the OverlapSphere, get pushed back by the AddExplosionForce
		foreach (Collider onHead in colliders) 
		{
			//if not on head just do whatever it was doing
			if (!onHead) 
			{
				continue;
			}
			
			//if on head
			if (onHead.rigidbody)
			{
				//bounce on head
				onHead.rigidbody.AddExplosionForce(power, annoyingPos, radius);
				int randomSide;
				//Either move to the left or right side after bouncing on head plumber style 
				randomSide = Random.Range(0,2);
				if(randomSide == 0)
				{
					onHead.rigidbody.AddForce (Vector2.right * power); //Move to the right side
				}
				if(randomSide == 1)
				{
					onHead.rigidbody.AddForce (new Vector2(-1,0) * power); //move to the left side
				}
			}
		}
	}

	public void alertOthers()
	{
		detected = true;
		losTimer= losMaxTimer;
	}


	void OnTriggerEnter(Collider other)
	{
		//If enemy collides with a waypoint and threat level is at 1 then go straight to idle state
		if(other.gameObject.CompareTag ("Enemy Waypoint") && !detected)
		{
			//Switch way point locations
			SwitchWaypoint ();
			//Reset idle timer
			idleTimer = idleMaxTimer;
			//Idle is set to true to stop the patrol function
			idle = true;
		}
	}

	void OnCollisionStay(Collision other)
	{
		//If on head
		if(other.gameObject.CompareTag ("Player") && PlayerGrounded.groundDetect == false)
		{
			//bounce off
			getOffMe();
			//also take damage PLUMBER style
			transform.gameObject.SendMessage ("applyEnemyDamage", 1f, SendMessageOptions.DontRequireReceiver); // and consume its health
		}
	}
}
