using UnityEngine;
using System.Collections;

//Script Objective: Button functions. Can be used in other scenes.

public class ButtonScript : MonoBehaviour 
{
	//Create public booleans to identify which button is which
	public bool playGame1 = false;
	public bool playGame2 = false;
	public bool playGame3 = false;
	public bool testLevel = false;
	public bool quitGame = false;
	public bool mainMenu = false;
	public bool resumeBtn = false;
	public bool levelSelect = false;

	public bool lastCheckpoint = false;

	public bool manualScene = false;

	public bool credit = false;

	// Use this for initialization
	void Start () 
	{
		//make sure cursor is on
		Screen.showCursor = true;
	}
	
	//When Mouse hovers on text
	void OnMouseEnter()
	{
		//Change the colour of the text
		renderer.material.color = new Color(0f,255f,255f);
	}

	void OnMouseExit()
	{
		//Change the colour of the text
		renderer.material.color = new Color(255f,255f,255f);
	}

	//When player clicks and release
	void OnMouseUp()
	{
		//If play game is clicked
		if(playGame1)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;

			//Make sure game over screen isn't on
			GameOverScript.gameOver = false;

			//Make sure the end level boolean is off
			StatScript.endLevel = false;

			//Make sure the Pause screen is off
			PlayerPause.paused = false;

			//Go to game
			Application.LoadLevel (1);
		}

		//If play game is clicked
		if(playGame2)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			//Make sure game over screen isn't on
			GameOverScript.gameOver = false;
			
			//Make sure the end level boolean is off
			StatScript.endLevel = false;
			
			//Make sure the Pause screen is off
			PlayerPause.paused = false;
			
			//Go to game
			Application.LoadLevel (2);
		}

		//If play game is clicked
		if(playGame3)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			//Make sure game over screen isn't on
			GameOverScript.gameOver = false;
			
			//Make sure the end level boolean is off
			StatScript.endLevel = false;
			
			//Make sure the Pause screen is off
			PlayerPause.paused = false;
			
			//Go to game
			Application.LoadLevel (3);
		}

		if(testLevel)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			//Make sure game over screen isn't on
			GameOverScript.gameOver = false;
			
			//Make sure the end level boolean is off
			StatScript.endLevel = false;
			
			//Make sure the Pause screen is off
			PlayerPause.paused = false;
			
			//Go to game
			Application.LoadLevel (4);
		}


		//if quit button is clicked
		if(quitGame)
		{
			//Quit the god damn game
			Application.Quit ();
		}

		if(mainMenu)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;

			//Go to Main menu
			Application.LoadLevel (0);
		}

		if(resumeBtn)
		{
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("Unpause");
		}

		if(lastCheckpoint && CheckpointScript.respawnPoint > 0)
		{
			GameObject.FindGameObjectWithTag ("Level Manager").SendMessage ("Respawn");
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("Unpause");
			GameObject.FindGameObjectWithTag ("Level Manager").SendMessage ("NotGameOver");

			//Shove all enemies into an array
			GameObject [] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
			
			for (int i = 0; i < enemies.Length; i++)
			{
				enemies[i].SendMessage ("applyEnemyDamage" , 1000f);
				enemies[i].SendMessage ("NaturalDeath");
			}
		}

		if(manualScene)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			

			Application.LoadLevel (5);
		}

		if(credit)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			
			Application.LoadLevel (6);
		}

		if(levelSelect)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			
			Application.LoadLevel (7);
		}
	}
}
