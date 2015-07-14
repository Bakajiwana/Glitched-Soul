using UnityEngine;
using System.Collections;

public class CyborgBullet : MonoBehaviour {
	
	public Transform bulletSpark; //Bullet spark prefab
	public Transform playerBlood;
	
	public float damage = 5f;

	//Gotta fix the launch fail, because it hits itself
	public float launchFixMaxTimer = 0.1f;
	private float launchFixTimer;
	
	// Use this for initialization
	void Start () 
	{
		launchFixTimer = launchFixMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		launchFixTimer -= Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision other)
	{
		//When the unarmed triggers hit the player shield: do damage
		if(other.gameObject.CompareTag ("Player Shield"))
		{
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("applyShieldDamage", damage); //Send message to damage shield
			//otherwise emit sparks at the hit point
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Player")) //Player hit
		{				
			Instantiate (playerBlood, transform.position, transform.rotation); //make it bleed
			other.gameObject.SendMessage ("applyPlayerDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);	
		}
		
		if(other.gameObject.CompareTag ("Environment") || other.gameObject.CompareTag ("Enemy")  && launchFixTimer <= 0f)
		{
			//otherwise emit sparks at the hit point
			Instantiate (bulletSpark, transform.position, transform.rotation);
			Destroy (gameObject);
		}

		if(other.gameObject.CompareTag ("Breakable"))
		{
			Instantiate (bulletSpark, transform.position, transform.rotation);
			other.gameObject.SendMessage ("applyObjectDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);
		}
	}
}

