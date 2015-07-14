using UnityEngine;
using System.Collections;

//The respawn script for the player

public class CheckpointScript : MonoBehaviour 
{
	public Transform player;
	static public Vector3 checkpointPos;

	public Transform[] enemyRespawnAreas;

	//Current respawns
	static public int respawnPoint;

	//Need to fix the part when breakable objects don't respawn.
	private GameObject [] breakables;

	public PlayerHealth health;

	// Use this for initialization
	void Start () 
	{
		//Shove all the breakable objects at the start of the game into an array to be re spawned later.
		breakables = GameObject.FindGameObjectsWithTag ("Breakable");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	//The respawn function should be called from the gameover screen
	public void Respawn()
	{
		health.playerHealth = health.playerMaxHealth;
		player.gameObject.SetActive (true);
		player.position = checkpointPos + Vector3.up * 3;
		GlitcherOverheat.heat = 0f;

		for (int i = respawnPoint - 1; i < enemyRespawnAreas.Length; i++)
		{
			enemyRespawnAreas [i].gameObject.SetActive (true);
		}

		for(int j = 0; j < breakables.Length; j++)
		{
			breakables [j].gameObject.SetActive (true);
		}

		StatScript.killScore = 0;
		StatScript.playerKill = PlayerPrefs.GetInt ("Checkpoint Kills");

		//Destroy all enemy bullets, missiles as well as players bullets
		GameObject [] playerBullets = GameObject.FindGameObjectsWithTag ("Player Bullet");
		GameObject [] enemyBullets = GameObject.FindGameObjectsWithTag ("Enemy Bullet");
		GameObject [] enemyMissiles = GameObject.FindGameObjectsWithTag ("Enemy Missile");

		for(int a = 0; a < playerBullets.Length; a++)
		{
			Destroy (playerBullets[a]);
		}

		for(int b = 0; b < enemyBullets.Length; b++)
		{
			Destroy (enemyBullets[b]);
		}

		for(int c = 0; c < enemyMissiles.Length; c++)
		{
			Destroy (enemyMissiles[c]);
		}

		player.rigidbody.useGravity = true;
		PlayerJumpPad.inJumpZone = false;
	}
}
