using UnityEngine;
using System.Collections;

//Script objective the grenade interaction

public class GrenadeScript : MonoBehaviour 
{
	public float damage = 100f;

	//Explosion variables
	public Transform grenadeExplosion;
	public float radius = 50f;
	public float power = 1500f;

	private bool timerStart = false;
	private float explosionTimer;
	public float explosionMaxTimer = 2f;

	// Use this for initialization
	void Start () 
	{
		explosionTimer = explosionMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the grenade hits an environment object then it will bounce etc and start count down and blow up
		if (timerStart)
		{
			explosionTimer -= Time.deltaTime;
		}

		//When timer runs out
		if(explosionTimer <= 0f)
		{
			AreaOfEffect ();
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag ("Environment"))
		{
			timerStart = true;
		}

		if(other.gameObject.CompareTag ("Breakable"))
		{
			timerStart = true;
		}

		if(other.gameObject.CompareTag ("Enemy"))
		{
			AreaOfEffect ();
		}
	}


	//This function is called to produce an area of effect, an explosion force and radius
	void AreaOfEffect()
	{
		//Tweak this code by using Rigidbody.AddExplosionForce to blow the enemies away
		//and also hurts the player 
		Vector3 explosionPos = transform.position;
		
		//Return all colliders into an array within the OverlapSphere radius
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		
		
		//For each collider inside the OverlapSphere, get pushed back by the AddExplosionForce
		foreach (Collider hit in colliders) 
		{
			if (!hit) 
			{
				continue;
			}
			
			if (hit.rigidbody)
			{
				hit.rigidbody.AddExplosionForce(power, explosionPos, radius);

				if(hit.rigidbody.CompareTag("Enemy"))
				{
					hit.gameObject.SendMessage ("applyEnemyDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
				}

				if(hit.rigidbody.CompareTag ("Breakable"))
				{
					hit.rigidbody.SendMessage ("applyObjectDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
				}
			}
		}

		//Instantiate an explosion
		Instantiate (grenadeExplosion, transform.position, transform.rotation);

		//Destroy itself
		Destroy(gameObject);
	}
}
