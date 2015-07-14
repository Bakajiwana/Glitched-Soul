using UnityEngine;
using System.Collections;

/* SCRIPT OBJECTIVES
 * Count and Calculate stats
 * End Level results will be stored in this script
 * Player Kill Count
 * Players Points
 * Timer
 * Shows end results 
*/ 

public class StatScript : MonoBehaviour 
{
	//Scoring variables
	static public int playerScore = 0;
	static public int killScore = 0;
	private int playerHighScore = 0;
	private string score;

	//Scoring GUI Variables
	public float scoreX;
	public float scoreY;
	public float scoreW;
	public float scoreH;

	private float scoreTextX;
	private float scoreTextY;
	private float scoreTextW;
	private float scoreTextH;

	public GUIStyle textStyle01;	//GUI style for text

	public int maxScoreSize;
	public int minScoreSize;
	public int scoreScaleSpeed = 3;
	public int scoreSize = 35;

	//Variable for levels
	public int level;

	//Timer Variables
	private float timer = 0.0f;
	private float min;
	private float sec;
	private float fraction;

	//Kill Count Variable
	static public int playerKill = 0;


	//End Level screen variables
	public TextMesh timerText;
	public TextMesh killText;
	public TextMesh scoreText;
	public TextMesh newHighScoreText;
	public TextMesh rankText;
	public Transform endLevelScreen;
	static public bool endLevel = false;

	public float killDelay;
	public float scoreDelay;
	public float newHighScoreDelay;
	public float rankDelay;

	//Rank Variables
	public int ssScore;
	public int sScore;
	public int aScore;
	public int bScore;
	public int cScore;
	public int dScore;


	// Use this for initialization
	void Start () 
	{
		//If the player is in the level specified, then get the high score of previous high scores
		if(level == 1)
		{
			playerHighScore = PlayerPrefs.GetInt ("Level01 Score");
		}

		if(level == 2)
		{
			playerHighScore = PlayerPrefs.GetInt ("Level02 Score");
		}

		if(level == 3)
		{
			playerHighScore = PlayerPrefs.GetInt ("Level03 Score");
		}

		killScore = 0;
		playerScore = 0;
		playerKill = 0;
	}

	void OnGUI()
	{
		//Initiate GUI variable results
		scoreTextX = Screen.width/ scoreX;
		scoreTextY = Screen.height/ scoreY;
		scoreTextW = Screen.width/ scoreW;
		scoreTextH = Screen.height / scoreH;

		//Scoring text
		int overallScore = playerScore + killScore;
		score = "Score:   " + overallScore;
		GUI.Label (new Rect (scoreTextX, scoreTextY, scoreTextW, scoreTextH), score, textStyle01); //this displays your health
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(endLevel == false)
		{
			//Calculate the time as the level starts
			timer += Time.deltaTime;
		}
		else
		{
			//make sure cursor is on
			Screen.showCursor = true;

			//Display time
			min = (timer/60f);
			sec = (timer % 60f);
			fraction = ((timer * 10) %10);
			timerText.text = string.Format("{0:00}:{1:00}:{2:00}",min,sec,fraction);

			if(killDelay < 0f)
			{
				killText.gameObject.SetActive (true);
				killText.text = "" + playerKill;
			}
			else
			{
				killDelay -= Time.deltaTime;
			}

			if(scoreDelay < 0f)
			{
				scoreText.gameObject.SetActive(true);
				scoreText.text = "" + playerScore ;
			}
			else
			{
				scoreDelay -= Time.deltaTime;
			}

			if(newHighScoreDelay < 0f)
			{
				//if score is more than highscore
				if(playerScore > playerHighScore && level == 1)
				{
					newHighScoreText.gameObject.SetActive (true);
					PlayerPrefs.SetInt ("Level01 Score", playerScore);
				}

				//if score is more than highscore
				if(playerScore > playerHighScore && level == 2)
				{
					newHighScoreText.gameObject.SetActive (true);
					PlayerPrefs.SetInt ("Level02 Score", playerScore);
				}

				//if score is more than highscore
				if(playerScore > playerHighScore && level == 3)
				{
					newHighScoreText.gameObject.SetActive (true);
					PlayerPrefs.SetInt ("Level03 Score", playerScore);
				}
			}
			else
			{
				newHighScoreDelay -= Time.deltaTime;
			}

			if(rankDelay < 0f)
			{
				rankText.gameObject.SetActive (true);

				if(playerScore >= ssScore)
				{
					rankText.text = "SS";
				}
				else if(playerScore >= sScore)
				{
					rankText.text = "S";
				}
				else if(playerScore >= aScore)
				{
					rankText.text = "A";
				}
				else if(playerScore >= bScore)
				{
					rankText.text = "B";
				}
				else if(playerScore >= cScore)
				{
					rankText.text = "C";
				}
				else if(playerScore >= dScore)
				{
					rankText.text = "D";
				}
			}
			else
			{
				rankDelay -= Time.deltaTime;
			}
		}

		//Score tweening
		textStyle01.fontSize = scoreSize;

		if(scoreSize > minScoreSize)
		{
			scoreSize -= scoreScaleSpeed;
		}
	}

	//When player earns points
	public void applyScore(int _score)
	{
		//Increment the amoint of points depending on set score
		playerScore += _score;

		scoreSize = maxScoreSize;
	}

	public void applyKillScore(int _score)
	{
		//Increment the amoint of points depending on set score
		killScore += _score;

		scoreSize = maxScoreSize;
	}

	//When player gets a kill
	public void applyKill()
	{
		//Increment the amount of points depending on set score
		playerKill++;
	}

	//This function is to control the end level screen
	public void endScreen()
	{
		//Show end level screen
		endLevelScreen.gameObject.SetActive (true);
		endLevel = true;
	}
}
