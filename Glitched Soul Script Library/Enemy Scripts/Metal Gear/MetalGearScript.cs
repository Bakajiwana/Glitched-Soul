using UnityEngine;
using System.Collections;

//Script Objective: This script is the main controls of the Metal Gear (Big Robot thing), metal gear isn't really the name, just haven't thought of it yet

public class MetalGearScript : MonoBehaviour 
{
	private Animator anim;				//A variable reference to the animator of the character

	//Line of Sight Variables
	private bool detected; 
	public float sightDistance = 20f;
	public Transform player;

	public float closeRange = 0f;
	public float medRange = 15f;
	public float longRange = 40f;

	private bool stop;

	public MetalGearShoot gun1;
	public MetalGearShoot gun2;

	//Where is the character in world space.
	//Position Variables
	public Transform self;

	public float speed = 5f;
	
	//Object Position Variables
	private Vector2 playerPos;
	private Vector2 selfPos;

	//Stomp variables
	private float stompTimer;
	public float stompMaxTimer = 0.75f;
	public float stompRadius = 5.0f;
	public float stompPower = 10.0f;

	//Health Variables
	public float enemyMaxHealth = 200f;
	private float enemyHealth;
	public float stealthHitMultiplier = 5f;
	private bool lastHitByPlayer; 
	public int points = 50;
	public GameObject deathSelf; //Whole object itself (including its own waypoints etc)

	//Location of player to enemy
	private float angle;

	//Death variables
	public Transform deathParticles;

	//Corruption variables - when player is corrupting enemy
	public float corruptDist = 20f;
	public Transform corruptParticle;

	//Initial position
	private Vector3 initialPosition;

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		enemyHealth = enemyMaxHealth;

		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//--------------LINE OF SIGHT----------------------
		//This enemy is hard to avoid so maybe just use a distance detect instead of a raycast
		if(player)
		{
			float playerDist = Vector3.Distance (player.position, transform.position);

			if(playerDist <= sightDistance)
			{
				detected = true;
				//Notify Music to turn into combat
				GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
			}
		}

		//Obtain the player position using the camera
		if(player)
		{
			playerPos = Camera.main.WorldToScreenPoint(player.position);
		}
		
		selfPos = Camera.main.WorldToScreenPoint(self.position);
		
		//Obtain the difference between the mouse and player coordinates
		selfPos.x = selfPos.x - playerPos.x;
		selfPos.y = selfPos.y - playerPos.y; 
		
		//Use the difference of x/y to find the angle using Atan2(x/y). 
		angle = Mathf.Atan2 (selfPos.x, selfPos.y) * Mathf.Rad2Deg; //This will return aiming coordinates

		//If the player is detected and still alive then attack and not corrupted
		if(PlayerUpgrade.corrupt == false)
		{
			//Turn off corrupt particles
			corruptParticle.gameObject.SetActive (false);

			if(detected && player)
			{
				//calculate distance of player
				float dist = Vector3.Distance (player.position, transform.position);

				if(dist >= longRange)
				{
					anim.SetBool ("Shoot", false);
					if(!stop)
					{
						if(angle < 0)
						{
							anim.SetFloat ("Speed", -1f);
							transform.Translate(-speed * Vector3.forward * Time.deltaTime);
						}
						else
						{
							anim.SetFloat ("Speed", 1f);
							transform.Translate(speed * Vector3.forward * Time.deltaTime);
						}
					}
					else
					{
						anim.SetFloat ("Speed", 0f);
					}
				}
				else if(dist >= medRange)
				{
					if(angle > 0)
					{
						gun1.Fire ();
						gun2.Fire ();
						anim.SetBool ("Shoot", true);
					}


					anim.SetFloat ("Speed", 0f);

					anim.SetLayerWeight (1, 1f);

					stompTimer = stompMaxTimer;
				}
				//if player is at close range
				else if(dist >= closeRange)
				{
					if(angle > 0)
					{
						gun1.Fire ();
						gun2.Fire ();
					}
					anim.SetBool ("Shoot", true);

					anim.SetLayerWeight (1, 0f);
					anim.SetTrigger ("Stomp");
					stompTimer -= Time.deltaTime;

					if(stompTimer <= 0f)
					{
						stompTimer = stompMaxTimer;

						Vector3 stompPos = transform.position;
						Collider [] colliders = Physics.OverlapSphere (stompPos, stompRadius);

						foreach (Collider hit in colliders)
						{
							if(hit && hit.rigidbody)
							{
								hit.rigidbody.AddExplosionForce (stompPower, stompPos, stompRadius, 3.0f);
							}
						}
					}
				}
			}
		}

		if(player && PlayerUpgrade.corrupt == true)
		{
			//calculate distance of player
			float playerDist = Vector3.Distance(player.position, transform.position);
			
			if(playerDist <= corruptDist)
			{
				anim.SetTrigger ("Stagger");
				corruptParticle.gameObject.SetActive (true);
			}
		}

		if(enemyHealth <= 0f)
		{
			enemyDeath();
		}
	}

	void enemyDeath()
	{
		//if the last damage was done by the player then, count as death and apply score
		if(lastHitByPlayer == true)
		{
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("applyKill");			//Apply kill stat
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("applyKillScore", points);	//Apply score
			transform.gameObject.SendMessage ("pickUp", SendMessageOptions.DontRequireReceiver);	//Drop item
		}
		detected = false;
		enemyHealth = enemyMaxHealth;
		//Instantiate ragdoll
		Instantiate (deathParticles, transform.position, transform.rotation);
		//Return to initial position, prepared for respawning
		transform.position = initialPosition;
		//Destroy self
		deathSelf.gameObject.SetActive (false);
	}


	//This function is called to take damage
	public void applyEnemyDamage (float _damage)
	{
		//If not detected then take 3x damage
		if(detected == false || angle < 0)
		{
			//Decrease health
			enemyHealth -= _damage * stealthHitMultiplier; //take damage with multiplier because not paying attention.
			detected = true;
		}
		else
		{
			enemyHealth -= _damage; //take damage
			detected = true;
		}		

		//Initiate stagger function
		anim.SetTrigger ("Stagger");

		//Notify Music to turn into combat
		GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
	}

	public void NaturalDeath()
	{
		lastHitByPlayer = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Restricted"))
		{
			stop = true;
		}

		//If hit by player projectile, or bullet...
		if(other.gameObject.CompareTag ("Player Bullet") || other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Player Weapon"))
		{
			//The last hit by player is now true and this enemy can die and add to the kill count and score
			lastHitByPlayer = true;
		}
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
}
