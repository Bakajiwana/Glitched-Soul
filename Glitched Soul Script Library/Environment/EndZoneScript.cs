using UnityEngine;
using System.Collections;

public class EndZoneScript : MonoBehaviour {

	//If player touches this object send a message to the stat manager to display stat to end level
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Player"))
		{
			GameObject.FindGameObjectWithTag ("Stat Manager").SendMessage ("endScreen");
			//Notify Music to turn into combat
			GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("EndLevel");
			Destroy (gameObject);

			StatScript.playerScore += StatScript.killScore;
			StatScript.killScore = 0;
		}
	}
}
