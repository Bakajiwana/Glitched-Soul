using UnityEngine;
using System.Collections;

public class PlayerArm : MonoBehaviour {

	//This script is to activate the player's "arm state" which is to hold his weapons.
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			other.gameObject.SendMessage ("Arm", 1f, SendMessageOptions.DontRequireReceiver);
			Destroy (gameObject);
		}
	}
}
