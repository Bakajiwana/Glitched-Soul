using UnityEngine;
using System.Collections;

public class MetalGearMissile : MonoBehaviour 
{
	public GameObject player;			//Player's variable
	public float speed = 5f;			//Speed of missile
	public float destroyTime = 10f;		//Time for which this missile will be destroyed

	public float playerDamage = 10f;
	public float enemyDamage = 5f;

	public Transform bulletSpark;
	public Transform playerBlood;

	//Gotta fix the launch fail, because it hits itself
	public float launchFixMaxTimer = 1f;
	private float launchFixTimer;

	// Use this for initialization
	void Start () 
	{
		//Destroy variable in a given time
		Destroy (gameObject, destroyTime);

		launchFixTimer = launchFixMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		//Move towards the player
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, step);

		//Look at Player
		Vector3 playerDir = player.transform.position - transform.position;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, playerDir, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);

		launchFixTimer -= Time.deltaTime;
	}

	public void Destroyed()
	{
		Instantiate (bulletSpark, transform.position, transform.rotation);
		Destroy (gameObject);
	}


	void OnCollisionEnter (Collision other)
	{
		if(other.gameObject.CompareTag ("Player Shield"))
		{
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("applyShieldDamage", playerDamage); //Send message to damage shield
			//otherwise emit sparks at the hit point
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Player")) //Player hit
		{				
			Instantiate (playerBlood, transform.position, transform.rotation); //make it bleed
			other.gameObject.SendMessage ("applyPlayerDamage", playerDamage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);	
		}

		if(other.gameObject.CompareTag ("Environment"))
		{
			//otherwise emit sparks at the hit point
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Enemy") && launchFixTimer <= 0f)
		{
			//otherwise emit sparks at the hit point
			Instantiate (bulletSpark, transform.position, transform.rotation);
			other.gameObject.SendMessage ("applyEnemyDamage", enemyDamage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Player Bullet"))
		{
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Breakable"))
		{
			Instantiate (bulletSpark, transform.position, transform.rotation);
			other.gameObject.SendMessage ("applyObjectDamage", playerDamage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag ("Player Bullet"))
		{
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
}
