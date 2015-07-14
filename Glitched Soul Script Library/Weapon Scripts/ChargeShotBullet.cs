using UnityEngine;
using System.Collections;

public class ChargeShotBullet : MonoBehaviour {

	public Transform bulletSpark; //Bullet spark prefab
	public Transform enemyBlood;
	
	public float damage = 5f;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Enemy")) //enemy hit
		{				
			Instantiate (enemyBlood, transform.position, transform.rotation); //make it bleed
			other.gameObject.SendMessage ("applyEnemyDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
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
		}
	}
}
