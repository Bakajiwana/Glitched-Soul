using UnityEngine;
using System.Collections;

//OBJECTIVE: Workaround for detecting the ground

public class PlayerGrounded : MonoBehaviour {

	//Variables
	public static bool groundDetect;	//Needs to be a static variable to communicate with another script


	//Collision function to detect ground
	void OnTriggerStay (Collider other)
	{
		//If the ground detector detects the environment or enemy
		if (other.gameObject.CompareTag("Environment"))
		{
			groundDetect = true; 	//The ground detector attached to the player is detecting ground
		}
		//If the ground detector detects the environment or enemy
		if (other.gameObject.CompareTag("Breakable"))
		{
			groundDetect = true; 	//The ground detector attached to the player is detecting ground
		}
	}

	//If the collider exits the ground 
	void OnTriggerExit(Collider other)
	{
		//If the ground detector does not detect the environment or enemy
		if (other.gameObject.CompareTag("Environment"))
		{
			groundDetect = false; 	//not grounded.
		}
		//If the ground detector does not detect the environment or enemy
		if (other.gameObject.CompareTag("Breakable"))
		{
			groundDetect = false; 	//not grounded.
		}
	}
}
