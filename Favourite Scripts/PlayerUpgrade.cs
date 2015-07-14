using UnityEngine;
using System.Collections;
//Script Objective: When the player picks up a power up he can store and use it

//There are 3 temporary upgrades at the moment that are dropped by enemies with a chance:
//Corrupt: disables all mechs and cyborgs within range
//Ward: Invincible for a limited time
//Speed: Increases movement speed over time

//The player can only hold one of each at a time because the game will be massively unbalanced if they can hold more of each

//Reference for using scroll wheel: http://answers.unity3d.com/questions/58515/mouse-scroll-weapon-change.html

public class PlayerUpgrade : MonoBehaviour 
{
	//Upgrade variables
	private int currentUpgrade = 0; 
	public int minUpgrade = 1;		//Minimum number is the one button so any less will be moved to 3
	public int maxUpgrade = 3;		//maximum number of upgrades at the moment
	
	private bool corruptReady = false;
	private bool wardReady = false;
	private bool speedReady = false;
	
	static public bool corrupt = false;
	static public bool ward = false;
	static public bool speed = false;
	
	private float corruptTimer;
	public float corruptMaxTimer = 5f;
	private float wardTimer;
	public float wardMaxTimer = 5f;
	private float speedTimer;
	public float speedMaxTimer = 5f;
	
	public Transform speedPrefab;
	
	public StatScript stat;

	public Transform corruptBlast;
	
	//HUD variables
	public Transform corruptSelection;
	public Transform corruptActive;
	public Transform wardSelection;
	public Transform wardActive;
	public Transform speedSelection;
	public Transform speedActive;
	
	//Sound
	public AudioClip powerUp;
	public AudioClip powerDown;
	public float powerUpVolume = 0.7f;
	
	
	// Update is called once per frame
	void Update () 
	{
		if(stat.level == 0)
		{
			corruptReady = true;
			wardReady = true;
			speedReady = true;
		}
		//Switch current upgrade
		if(Input.GetKeyDown (KeyCode.Tab) || Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			changeUpgradeBackward ();
		}
		
		if(Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			changeUpgradeForward();
		}
		
		//Active HUD displays
		if(corruptReady)
		{
			corruptActive.gameObject.SetActive (false);
		}
		else 
		{
			corruptActive.gameObject.SetActive (true);
		}
		
		
		if(wardReady)
		{
			wardActive.gameObject.SetActive (false);
		}
		else 
		{
			wardActive.gameObject.SetActive (true);
		}
		
		
		if(speedReady)
		{
			speedActive.gameObject.SetActive (false);
		}
		else 
		{
			speedActive.gameObject.SetActive (true);
		}
		
		//If the player picks up a power up and there is no other power up then automatically have it selected for quick use
		if(corruptReady && !wardReady && !speedReady)
		{
			currentUpgrade = 1;
			corruptSelection.gameObject.SetActive (true);
			wardSelection.gameObject.SetActive (false);
			speedSelection.gameObject.SetActive (false);
		}
		
		if(!corruptReady && wardReady && !speedReady)
		{
			currentUpgrade = 2;
			corruptSelection.gameObject.SetActive (false);
			wardSelection.gameObject.SetActive (true);
			speedSelection.gameObject.SetActive (false);
		}
		
		if(!corruptReady && !wardReady && speedReady)
		{
			currentUpgrade = 3;
			corruptSelection.gameObject.SetActive (false);
			wardSelection.gameObject.SetActive (false);
			speedSelection.gameObject.SetActive (true);
		}
		
		
		switch (currentUpgrade)
		{
		case 1: //---------------Corrupt Ability------------------
			if(corruptReady == true && Input.GetKeyDown (KeyCode.Z) || corruptReady == true && Input.GetMouseButtonDown (2))
			{
				//Activate the ability
				corrupt = true;
				//Reset timer
				corruptTimer = corruptMaxTimer;
				//Player used up his stored corrupt ability
				corruptReady = false;
				//Change selection
				changeUpgradeForward ();
				
				//Play the activate sound
				audio.Stop ();
				audio.PlayOneShot (powerUp, powerUpVolume);

				Transform flash = Instantiate (corruptBlast, transform.position, transform.rotation) as Transform;
				flash.transform.parent = transform;
			}
			break;
			
		case 2: //----------------Ward Ability--------------------
			if(wardReady == true && Input.GetKeyDown (KeyCode.Z) || wardReady == true && Input.GetMouseButtonDown (2))
			{
				//Activate the ability
				ward = true;
				//Reset timer
				wardTimer = wardMaxTimer;
				//Player used up his stored corrupt ability
				wardReady = false;
				//Change selection
				changeUpgradeForward ();
				
				//Play the activate sound
				audio.Stop ();
				audio.PlayOneShot (powerUp, powerUpVolume);
			}
			
			break;
			
		case 3: //----------------Speed Ability-------------------
			if(speedReady == true && Input.GetKeyDown (KeyCode.Z) || speedReady == true && Input.GetMouseButtonDown (2))
			{
				//Activate the ability
				speed = true;
				//Reset timer
				speedTimer = speedMaxTimer;
				//Player used up his stored corrupt ability
				speedReady = false;
				//Change selection
				changeUpgradeForward ();
				
				//Play the activate sound
				audio.Stop ();
				audio.PlayOneShot (powerUp, powerUpVolume);
			}
			break;
		}
		
		//When an ability is true, start a countdown and when the countdown is finished, deactivate that ability
		if(corrupt)
		{
			corruptTimer -= Time.deltaTime;
		}
		
		if(corruptTimer <= 0f)
		{
			corrupt = false;
		}
		
		if(ward)
		{
			wardTimer -= Time.deltaTime;
		}
		
		
		if(wardTimer <= 0f)
		{
			ward = false;
		}
		
		if(speed)
		{
			speedTimer -= Time.deltaTime;
			speedPrefab.gameObject.SetActive (true);
		}
		
		if(speedTimer <= 0f)
		{
			speedPrefab.gameObject.SetActive (false);
			speed = false;
		}
	}
	
	public void CorruptReady()
	{
		if(!corruptReady)
		{
			corruptReady = true;
		}
		else if(!wardReady)
		{
			wardReady = true;
		}
		else if (!speedReady)
		{
			speedReady = true;
		}
	}
	
	public void WardReady()
	{
		if(!wardReady)
		{
			wardReady = true;
		}
		else if (!speedReady)
		{
			speedReady = true;
		}
		else if(!corruptReady)
		{
			corruptReady = true;
		}
	}
	
	public void SpeedReady()
	{
		if (!speedReady)
		{
			speedReady = true;
		}
		else if(!corruptReady)
		{
			corruptReady = true;
		}
		else if(!wardReady)
		{
			wardReady = true;
		}
	}
	
	void changeUpgradeForward()
	{
		if(currentUpgrade == 0)
		{
			if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 1)
		{
			if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 2)
		{
			if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 3)
		{
			if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
	}
	
	void changeUpgradeBackward()
	{
		if(currentUpgrade == 0)
		{
			if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 1)
		{
			if(speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else if (wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 2)
		{
			if (corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else if(wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
		
		else if(currentUpgrade == 3)
		{
			if (wardReady)
			{
				currentUpgrade = 2;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (true);
				speedSelection.gameObject.SetActive (false);
			}
			else if(corruptReady)
			{
				currentUpgrade = 1;
				corruptSelection.gameObject.SetActive (true);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
			else if (speedReady)
			{
				currentUpgrade = 3;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (true);
			}
			else
			{
				currentUpgrade = 0;
				corruptSelection.gameObject.SetActive (false);
				wardSelection.gameObject.SetActive (false);
				speedSelection.gameObject.SetActive (false);
			}
		}
	}
}
