using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour 
{

	public AudioClip combatMusic;
	public AudioClip ambientMusic;

	private float combatTimer;
	private bool combat = false;
	public float combatMaxTimer = 20f;

	private bool volumeIncrease;
	private float volumeSpeed = 0f;
	private float maxVolume = 0.5f;

	private bool ambientOneShot = false;
	
	// Update is called once per frame
	void Update () 
	{
		if(combatTimer >= 0f)
		{
			combat = true;
			combatTimer -= Time.deltaTime;
		}
		else 
		{
			combat = false;
		}

		if(!combat && !ambientOneShot)
		{
			audio.Stop ();
			audio.clip = ambientMusic;
			audio.Play ();
			ambientOneShot = true;
		}

		if(volumeIncrease && volumeSpeed < maxVolume)
		{
			audio.volume = volumeSpeed;
			volumeSpeed += Time.deltaTime;
		}
	}

	public void CombatEngaged()
	{

		combatTimer = combatMaxTimer;

		if(!combat)
		{
			audio.Stop ();
			audio.clip = combatMusic;
			audio.Play ();
		}

		ambientOneShot = false;
	}


	public void Volume()
	{
		volumeIncrease = true;
	}
}
