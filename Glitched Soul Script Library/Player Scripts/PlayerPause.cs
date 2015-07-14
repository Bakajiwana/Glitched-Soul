using UnityEngine;
using System.Collections;

public class PlayerPause : MonoBehaviour 
{
	//To let the player pause

	//Pause Variables
	public Transform pauseScreen;

	//Going to have to disable the player rotation and shooting scripts
	static public bool paused = false;

	public PlayerRotation rotation;

	// Use this for initialization
	void Start () 
	{
		//Get Script components
		rotation = GetComponent<PlayerRotation>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Escape))
		{
			if(Time.timeScale == 1 && StatScript.endLevel == false && GameOverScript.gameOver == false)
			{
				Pause();
			}
			else 
			{
				Unpause();
			}
		}
	}

	void Pause()
	{
		pauseScreen.gameObject.SetActive (true);
		//make sure cursor is on
		Screen.showCursor = true;
		
		//Pause the game and display Paused text
		//"One little tip I want to throw in, and this is only 
		//sometimes handy, but for me, sometimes setting Time.timeScale to 0 can give you errors 
		//or screw up the game in some way. For example, if you have any variable in your game 
		//divided by Time.timeScale, you'll get back an error because you're dividing by zero. 
		//So my trick is to set Time.timeScale to 0.0000001. That way, the game is technically moving, 
		//but it's moving so slow that you will never see anything change. 
		//It would literally take a million seconds for 1 second in real time to pass."
		Time.timeScale = 0.0000001f;
		
		//Disable rotations to stop player rotating like an idiot
		rotation.enabled = false;
		
		paused = true;
	}

	public void Unpause()
	{
		//Set time scale to normal
		Time.timeScale = 1;
		pauseScreen.gameObject.SetActive (false);
		//make sure cursor is off
		Screen.showCursor = false;
		
		rotation.enabled = true;
		
		paused = false;
	}
}
