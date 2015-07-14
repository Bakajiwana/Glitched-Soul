using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			other.gameObject.SendMessage ("death", SendMessageOptions.DontRequireReceiver); // Kill the player
		}
	}
}
