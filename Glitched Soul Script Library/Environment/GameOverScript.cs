using UnityEngine;
using System.Collections;

//Display Game Over Screen

public class GameOverScript : MonoBehaviour 
{
	//game over variables
	static public bool gameOver = false;
	public Transform gameOverScreen;
	private float gameOverTimer = 0f;
	public float gameOverMaxTimer = 1f;

	void Update()
	{
		//Set a timer so the player can see the player die before showing game over screen and accidently pressing a button
		if(gameOver)
		{
			if(gameOverTimer > 0f)
			{
				gameOverTimer -= Time.deltaTime;
			}
			else
			{
				gameOverScreen.gameObject.SetActive (true);
			}
		}
		else
		{
			gameOverScreen.gameObject.SetActive (false);
		}
	}

	public void GameOver()
	{
		gameOverTimer = gameOverMaxTimer;
		gameOver = true;
	}	

	public void NotGameOver()
	{
		gameOver = false;
	}
}
