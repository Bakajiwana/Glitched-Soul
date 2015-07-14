using UnityEngine;
using System.Collections;

public class CyborgSword : MonoBehaviour {

	public float damage = 5f;

	public CyborgMeleeScript meleeScript;
	
	void OnTriggerEnter (Collider other)
	{
		if(meleeScript.melee == true)
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
}
