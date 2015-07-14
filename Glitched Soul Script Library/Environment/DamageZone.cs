using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour 
{
	public float damage = 2f;

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			other.gameObject.SendMessage ("applyPlayerDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
	}
}
