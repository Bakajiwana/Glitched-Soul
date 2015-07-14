using UnityEngine;
using System.Collections;

public class GlitcherBullet : MonoBehaviour {

	public Transform bulletSpark; //Bullet spark prefab
	public Transform enemyBlood;
	
	public float damage = 5f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag ("Enemy")) //enemy hit
		{				
			Instantiate (enemyBlood, transform.position, transform.rotation); //make it bleed
			other.gameObject.SendMessage ("applyEnemyDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
			Destroy (gameObject);	
		}
		
		if(other.gameObject.CompareTag ("Environment"))
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
