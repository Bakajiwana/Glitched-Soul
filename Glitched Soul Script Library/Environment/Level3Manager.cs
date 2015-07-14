using UnityEngine;
using System.Collections;

//Controls the events happening in level 3

public class Level3Manager : MonoBehaviour 
{
	//When player reaches a certain amount of kills contact the object to be set inactive and instantiate explosion
	//But if the player spawns and kills is reset then the particle background should re appear
	public Transform section1;
	public Transform section2;
	public Transform section3;

	public int section1MaxKills;
	public int section2MaxKills;
	public int section3MaxKills;

	public level3Explosion explosion1;
	public level3Explosion explosion2;
	public level3Explosion explosion3;

	//These one shot variables stop extra commands to call for the explosion even when the scripts are set inactive
	private bool exp1OneShot;
	private bool exp2OneShot;
	private bool exp3OneShot;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//----------------------SECTION 1 ----------------------
		//If player kills has more then section x max kills then create the explosion
		if(StatScript.playerKill >= section1MaxKills && exp1OneShot == false)
		{
			explosion1.DataOverload ();
			section1.gameObject.SetActive(false);
			exp1OneShot = true; 
		}

		//If the player spawns and has less kills then set the sections back to true and one shot back to false
		if(StatScript.playerKill < section1MaxKills)
		{
			section1.gameObject.SetActive (true);
			exp1OneShot = false;
		}

		//----------------------SECTION 2 ----------------------
		//If player kills has more then section x max kills then create the explosion
		if(StatScript.playerKill >= section2MaxKills && exp2OneShot == false)
		{
			explosion2.DataOverload ();
			section2.gameObject.SetActive(false);
			exp2OneShot = true; 
		}
		
		//If the player spawns and has less kills then set the sections back to true and one shot back to false
		if(StatScript.playerKill < section2MaxKills)
		{
			section2.gameObject.SetActive (true);
			exp2OneShot = false;
		}


		//----------------------SECTION 3 ----------------------
		//If player kills has more then section x max kills then create the explosion
		if(StatScript.playerKill >= section3MaxKills && exp3OneShot == false)
		{
			explosion3.DataOverload ();
			section3.gameObject.SetActive(false);
			exp3OneShot = true; 
		}
		
		//If the player spawns and has less kills then set the sections back to true and one shot back to false
		if(StatScript.playerKill < section3MaxKills)
		{
			section3.gameObject.SetActive (true);
			exp3OneShot = false;
		}

		//SECTION 4 WILL BE THE END SECTION SO IF THE PLAYERS MISS ALL THE ENEMIES, THEN GAME CAN'T END
		//SO A SEPERATE SCRIPT WILL COUNT THE SECTION 4 ENEMIES IN A SCRIPT ARRAY AND THEN WHEN ALL ENEMIES
		//IN THAT ARRAY ARE KILLED THEN GAME ENDS
		//CONTINUES IN Level3EndScript
	}
}
