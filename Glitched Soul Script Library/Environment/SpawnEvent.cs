using UnityEngine;
using System.Collections;

//When player runs into trigger, spawn the selected transform

public class SpawnEvent : MonoBehaviour 
{
	public GameObject[] objectSpawn;
	

	//If player runs into trigger the selected public transform should then be set active with its contents 
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			//Shove all enemies into an array
			GameObject [] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			
			for (int i = 0; i < enemies.Length; i++)
			{
				enemies[i].SendMessage ("applyEnemyDamage" , 1000f, SendMessageOptions.DontRequireReceiver);
				enemies[i].SendMessage ("NaturalDeath", SendMessageOptions.DontRequireReceiver);
			}

			for (int i = 0; i < objectSpawn.Length; i++)
			{
				objectSpawn[i].gameObject.SetActive (true);
			}

			transform.gameObject.SetActive (false);
		}
	}
}
