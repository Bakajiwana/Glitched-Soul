using UnityEngine;
using System.Collections;

//SECTION 4 WILL BE THE END SECTION SO IF THE PLAYERS MISS ALL THE ENEMIES, THEN GAME CAN'T END
//SO A SEPERATE SCRIPT WILL COUNT THE SECTION 4 ENEMIES IN A SCRIPT ARRAY AND THEN WHEN ALL ENEMIES
//IN THAT ARRAY ARE KILLED THEN GAME ENDS
//CONTINUES IN Level3EndScript

public class Level3End : MonoBehaviour 
{
	public bool end = false;

	public int minEnemiesLeft;


	public EndScreen gameEnd;

	public Transform endParticle;
	public Transform endExplosion;
	
	// Update is called once per frame
	void Update () 
	{
		if(end)
		{
			//Shove all enemies into an array
			GameObject [] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

			if(enemies.Length <= minEnemiesLeft)
			{
				gameEnd.endFade();
				endParticle.gameObject.SetActive (false);
				endExplosion.gameObject.SetActive (true);
			}
		}


	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			end = true;
		}
	}
}
