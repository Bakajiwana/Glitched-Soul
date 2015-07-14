using UnityEngine;
using System.Collections;

//Script objective: when the enemy slaps the player it will do damage. 

public class CyborgUnarmedMelee : MonoBehaviour 
{
	public float damage = 5f;

	void OnTriggerEnter (Collider other)
	{
		//When the unarmed triggers hit the player shield: do damage
		if(other.gameObject.CompareTag ("Player Shield"))
		{
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("applyShieldDamage", damage); //Send message to damage shield
		}

		//When the unarmed triggers hit the player
		if(other.gameObject.CompareTag ("Player"))
		{
			other.SendMessage ("applyPlayerDamage", damage, SendMessageOptions.DontRequireReceiver); //Send message to damage player health
		}
	}
}
