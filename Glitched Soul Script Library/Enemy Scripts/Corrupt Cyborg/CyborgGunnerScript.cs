using UnityEngine;
using System.Collections;

//Script Objective: Control the corrupted cyborg.
//Using booleans to change animations
//Get him to move between points using his own personal navPoint cube. The Nav cube should move to certain points
//if the player is spotted the nav cube should teleport to the player as the new target. 

/* References:
 * LookAt to Only Rotate on Y Axis - http://answers.unity3d.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html
*/ 

public class CyborgGunnerScript : MonoBehaviour 
{
	//Variables
	//The threat level integer will control different phases of:
	//Patrol, Alert, Attack
	private int threatLevel = 1;

	//Mecanim variables
	public float anim_speed = 1f;		//Sets the speed of all the animations being played	
	private Animator anim;				//A variable reference to the animator of the character

	//Movement variables
	private float walkSpeed = 0.3f;
	public float runSpeed = 1f; 
	public float idleSpeed = 0f; 

	//Line of Sight Variables
	public float sightDistance = 10;
	public bool detected = false;
	public float lineOfSightMaxTimer = 4f;
	[System.NonSerialized]	// Don't want to see in inspector
	public float lineOfSightTimer = 0f; 
	public Transform raycastPosition;

	//Idle variables
	private bool idle = false;
	private float idleTimer;
	public float idleMaxTimer = 5f;

	//Destination variables
	public Transform destination; 
	public Transform player;
	public Transform[] wayPoint; 
	private int currentWayPoint = 0;
	public Transform patrolZone;

	//AI Behavious adjustments
	public bool elite = false;
	public float minPatrolDist = 30f;

	//Pause Variables
	private float pauseTimer;
	public float pauseMaxTimer = 1f;

	//Alert Variables
	public float alertRadius; 

	//Attack Variables
	private bool attack = false;
	[System.NonSerialized]	// Don't want to see in inspector
	public bool shoot = false;	//Don't delete, this isn't useful in this script. But it is used in the nav script.
	public float attDist = 5f;
	public float shootMaxTimer = 3f;
	private float shootTimer;

	//Hit Variables
	private float hitTimer;
	public float hitMaxTimer;
	public bool hit = false; 


	public float corruptDist = 20f;
	public Transform corruptParticle;

	//Fix nav point getting stuck
	private bool navFix;
	private float navFixTimer;
	private float navFixMaxTimer = 0.6f;

	//When player is on head, there should be a get the fuck off effect.
	public float radius = 200.0F;
	public float power = 1500.0F;

	//Rotation variable
	public float damping = 5f;

	//Shooting variables
	public CyborgShotgun fireGun;

		
	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		//pause timer initiate
		pauseTimer = pauseMaxTimer;

		//Initiate hit timer
		hitTimer = hitMaxTimer;
	}


	// Update is called once per frame
	void Update () 
	{

		//--------------LINE OF SIGHT----------------------
		//Use a raycast for line of sight 
		//REFERENCE: Unity Doc on Raycasting
		int layerMask = 1 << 9;
		layerMask = ~layerMask; //Collide with anything but layer 9
		RaycastHit lineOfSight;
		Vector3 fwd = transform.TransformDirection (Vector3.forward);
		if(Physics.Raycast (raycastPosition.position, fwd, out lineOfSight, sightDistance, layerMask))
		{
			//calculate distance to player
			float distanceToPlayer = lineOfSight.distance;
			//If the raycast hits the player he is then detected
			if (lineOfSight.transform.CompareTag ("Player") && distanceToPlayer <= sightDistance)
			{
				//The player has been detected
				detected = true;
				//Reset the lineOfSightTimer
				lineOfSightTimer = lineOfSightMaxTimer;
				//Notify Music to turn into combat
				GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
			}
			//but if the enemy loses sight, set a countdown to line of sight becoming false
			else 
			{
				//if the player was detected and just escaped the line of sight of the enemy start countdown
				if (detected == true)
				{
					lineOfSightTimer -= Time.deltaTime; 
				}
			}

			//If the line of sight timer is lesser than 0. Then the player is not detected.
			if (lineOfSightTimer <= 0f)
			{
				detected = false;
			}
		}


		//------------------THREAT LEVEL CONTROLS---------------------
		//If the player is still alive then commence threat level parameters else he is dead and should return to patrolling
		if(player)
		{
			//If Player is not detected, threat level is 1
			if(!detected)
			{
				threatLevel = 1;
			}

			//If the player is detected, then proceed to next threat levels
			//Calculate the distance between the patrol zone and the player and if player is within player patrol commence attack
			float patrolDist = Vector3.Distance (patrolZone.position, player.position);
			if (patrolDist >= minPatrolDist)
			{
				detected = false;
				threatLevel = 1; 
			}
			else 
			{
				if(detected && !elite)
				{
					threatLevel = 2;

				}
				else if(detected && elite)
				{
					threatLevel = 3;
				}
			}
		}
		else
		{
			shoot = false;
			detected = false;
			threatLevel = 1;
		}

		if(attack)
		{
			Charge();								//CHARGE!!!!!!!!!!
			anim.SetLayerWeight(3, 1);
			anim.SetBool ("Aiming", true);
		}
		else
		{
			anim.SetLayerWeight(3, 0);
			anim.SetBool ("Aiming", false);
		}

		//If nav point is stuck because player touched the enemy.
		if(navFix)
		{
			navFixTimer -= Time.deltaTime;
			if(player)
			{
				destination.position = player.position;
			}
		}

		if(navFix == true && navFixTimer <= 0f)
		{
			navFix = false;
		}

		//Threat level 3 is activated when the threat level 2 pause is finished
		if(pauseTimer <= 0f)
		{
			anim.SetBool ("Alert", false);	//play alert animation
			
			if(detected)
			{
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
				threatLevel = 3;  //Continue attack
			}
			else
			{
				threatLevel = 1; 	//back to patrolling
			}
		}


		//If the hit boolean is true a countdown should commence to deactivate the boolean so it can be used again
		if (hit)
		{
			Hit();
		}		

		//If the enemy isn't corrupted
		if(PlayerUpgrade.corrupt == false)
		{
			anim.SetBool ("Corrupt", false);
			corruptParticle.gameObject.SetActive (false);
			//Use a switch case to control Enemy behaviour
			switch (threatLevel) 
			{
			case 1: //-----------------THREAT LEVEL 1----------------------
				//In threat level 1, we want this enemy to patrol between points and keep an eye out for the player
				Patrol();						//Patrol the area, between waypoints
				//Reset things
				anim.SetBool ("Alert", false);
				anim.SetBool ("PlayerJumped", false); 
				pauseTimer = pauseMaxTimer;
				attack = false;
				break;
			case 2: //-----------------THREAT LEVEL 2----------------------
				//In threat level 2, this is mainly an alert timer for the enemy to figure out if player is an enemy
				//Pause play alert animation
				anim.SetBool ("Alert", true);	//play alert animation
				Pause();	//stop
				attack = false;
				break;
			case 3: //-----------------THREAT LEVEL 3----------------------
				//In threat level 3, the enemy charges at the player and attacks
				elite = true;							//ELITE response, which basically means he will attack on sight for now on.
				anim.SetBool ("Alert", false);			//Detected Animation (Points at player, figuring out if he's an enemy)
				attack = true;
				break;
			}
		}

		if(player && PlayerUpgrade.corrupt == true)
		{
			//calculate distance of player
			float playerDist = Vector3.Distance(player.position, transform.position);

			if(playerDist <= corruptDist)
			{
				anim.SetBool ("Corrupt", true);
				corruptParticle.gameObject.SetActive (true);
				destination.position = transform.position;
			}
		}
	}


	//This function is called in threat level 1. This is where the player will walk towards a waypoint
	void Patrol()
	{
		if(!idle)	//If not idle
		{
			anim.SetFloat ("Speed", walkSpeed);	//Set to Walk animation
			destination.position = wayPoint[currentWayPoint].position;
		}
		else    	//If idle then initiate idle function
		{
			Idle();
		}
	}

	//The idle function
	void Idle()
	{
		anim.SetFloat ("Speed", idleSpeed); //Set idle animation

		//while idle is true the idleTimer is counting down
		idleTimer -= Time.deltaTime;


		//If the idleTimer is lesser than 0
		if(idleTimer <= 0f)
		{
			//Idle is false and a new destination shall be added
			SwitchWaypoint();
			idle = false;
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


	//This function is called when the enemy is required to stop for a while and look at the player
	void Pause()
	{
		//Pause countdown
		pauseTimer -= Time.deltaTime; 

		anim.SetFloat ("Speed", idleSpeed); //stop moving

		anim.SetBool ("Shoot", false); //reset shoot anim boolean

		shoot = false;

		//Stay where you are
		destination.position = transform.position;
	}


	//This function is called when charging at the player
	void Charge()
	{
		if(player)
		{
			destination.position = player.position;
		
			//calculate distance of player
			float playerAttDist = Vector3.Distance(player.position, transform.position);

			//If enemy is at attacking distance the enemy will shoot but if the player is too fire away he will move closer
			if(playerAttDist > attDist)
			{
				//Make your way to the player
				anim.SetFloat ("Speed", runSpeed);
				shoot = false;
			}
			else
			{
				//stop and shoot
				anim.SetFloat ("Speed", idleSpeed);
				Shoot ();
			}
		}
	}
	


	//This function is called when the player plays a hit animation
	void Hit()
	{
		hitTimer -= Time.deltaTime;

		anim.SetBool ("Hit", true);

		if(hitTimer <= 0f)
		{
			anim.SetBool ("Hit", false);
			hit = false;
			hitTimer = hitMaxTimer;
		}
	}

	//This function is called when attacking the player
	void Shoot()
	{
		//Reset pause timer
		pauseTimer = pauseMaxTimer;
		
		shoot = true;	//the enemy is now shooting
		
		shootTimer -= Time.deltaTime; //Shoot loop countdown
		
		//Fire and reset
		if(shoot == true && shootTimer <= 0f)
		{
			shootTimer = shootMaxTimer;
			anim.SetTrigger ("Recoil");
			fireGun.Fire ();
		}
		
		//Aim on the side the player is in
		Vector3 lookPos = player.position - transform.position;
		lookPos.y = 0;
		
		Quaternion rotation = Quaternion.LookRotation (lookPos);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
	}

	public void alertOthers()
	{
		detected = true;
		lineOfSightTimer= lineOfSightMaxTimer;
		elite = true;
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


	void OnTriggerEnter(Collider other)
	{
		//If enemy collides with a waypoint and threat level is at 1 then go straight to idle state
		if(other.gameObject.CompareTag ("Enemy Waypoint") && threatLevel == 1)
		{
			//Reset idle timer
			idleTimer = idleMaxTimer;
			//Idle is set to true to stop the patrol function
			idle = true;
		}
	}

	void OnCollisionEnter (Collision other)
	{
		if(other.gameObject.CompareTag ("Player") && PlayerUpgrade.corrupt == false)
		{
			//This is to fix the nav point when the player hops on his head
			destination.position = player.position;
			navFix = true;
			navFixTimer = navFixMaxTimer;
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
			transform.gameObject.SendMessage ("applyEnemyDamage", 5f, SendMessageOptions.DontRequireReceiver); // and consume its health
		}
	}
}
