using UnityEngine;
using System.Collections;

//Script Objective: To control health of player

//Script sources: Digital Tutors: Scripting the Health Bar and Andrew Adams: Unity3D with James- Custom Health Bars

//WARNING: FONT IS USED FOR PERSONAL USE AND MUST BE BOUGHT IF TO BE USED COMMERCIALLY!!!!!
//NOTE THE SHIELD GUI WILL BE DISPLAYED ON THE PLAYERHEALTH SCRIPT TO RETAIN PERFECT POSITION
public class PlayerHealth : MonoBehaviour 
{
	//Health Variables
	public float playerHealth;
	private float playerMinHealth = 0.01f; //don't let the min be 0 of the cut off material will die.
	public float playerMaxHealth = 100f;
	private string hp;

	private Animator anim;				//A variable reference to the animator of the character
	private float hitTimer;
	public float hitMaxTimer = 0.1f;

	//GUI Variables
	public float healthX;
	public float healthY;
	public float healthW;
	public float healthH;

	private float healthLeft;
	private float healthUp;
	private float healthWidth;
	private float healthHeight;

	public float shieldX;
	public float shieldY;
	public float shieldW;
	public float shieldH;

	private float shieldLeft;
	private float shieldUp;
	private float shieldWidth;
	private float shieldHeight;

	public Texture2D healthBar;
	public Material healthMat;
	public Texture2D healthBorder;
	public Texture2D shieldBar;
	public Texture2D shieldBorder;
	public Material shieldMat;

	//Ragdoll upon death
	public Transform ragdoll;

	public Transform wardPrefab;

	//Animation zoom attached to camera script
	public CameraHitZoom hitZoom;

	public Transform hitScreen;


	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		//Initiate health
		playerHealth = playerMaxHealth;
	}

	void OnGUI()
	{
		//Initiate GUI variable results
		healthLeft = Screen.width/ healthX;
		healthUp = Screen.height/ healthY;
		healthWidth = Screen.width/ healthW;
		healthHeight = Screen.height / healthH;

		shieldLeft = healthLeft;
		shieldUp = Screen.height/ shieldY;
		shieldWidth = healthWidth;
		shieldHeight = Screen.height / shieldH;

		//Health Bar
		Rect square = new Rect (healthLeft, healthUp, healthWidth, healthHeight);
		Graphics.DrawTexture (square, healthBar, healthMat);

		//Shield Bar
		Rect shieldBox = new Rect (shieldLeft, shieldUp, shieldWidth, shieldHeight);
		Graphics.DrawTexture (shieldBox, shieldBar, shieldMat);

		//Health Border
		GUI.DrawTexture (new Rect(healthLeft, healthUp, healthWidth, healthHeight), healthBorder, ScaleMode.StretchToFill, true, 1f);

		//Shield Border
		GUI.DrawTexture (new Rect(shieldLeft, shieldUp, shieldWidth, shieldHeight), shieldBorder, ScaleMode.StretchToFill, true, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Adjust the health values to prevent negatives and/ or extra health being calculated
		if(playerHealth < playerMinHealth)
		{
			playerHealth = playerMinHealth;
		}

		if(playerHealth > playerMaxHealth)
		{
			playerHealth = playerMaxHealth;
		}

		//Calculate health percentage
		float healthPercent = 1f - (playerHealth/ playerMaxHealth);
		if(healthPercent <= 0f)
		{
			healthPercent= 0.001f;
		}
		//Set the player health to affect the transparency of the Health bar HUD
		healthMat.SetFloat ("_Cutoff", healthPercent);

		//Calculate display shield percentage
		float shieldPercent = 1f - (PlayerShield.shieldDisplay/ 100f);
		if(shieldPercent <= 0f)
		{
			shieldPercent = 0.001f;
		}

		//Set the shield to affect the transparency of the shield bar HUD
		shieldMat.SetFloat ("_Cutoff", shieldPercent);

	// GAME OVER WHEN PLAYER HEALTH RUNS OUT
		if(playerHealth == playerMinHealth)
		{
			PlayerJumpPad.inJumpZone = true;
			playerHealth = playerMaxHealth;
			hitScreen.gameObject.SetActive (false);
			//Send message to level manager that it is game over
			GameObject.FindGameObjectWithTag ("Level Manager").SendMessage ("GameOver");
			//Destroy this game object
			transform.gameObject.SetActive (false);
			//Instantiate ragdoll
			Instantiate (ragdoll, transform.position, transform.rotation);
		}

		//------------IF PLAYER TAKES DAMAGE SHOW A HIT ANIMATION------------------
		if(anim.GetBool ("Hit") == true)
		{
			hitTimer -= Time.deltaTime;
		}

		if(hitTimer <= 0f)
		{
			anim.SetBool("Hit", false);
			hitScreen.gameObject.SetActive (false);
		}

		if(PlayerUpgrade.ward)
		{
			wardPrefab.gameObject.SetActive (true);
		}
		else
		{
			wardPrefab.gameObject.SetActive (false);
		}
	}

	//This function is called when player takes damage
	public void applyPlayerDamage(float _damage)
	{
		if(!PlayerUpgrade.ward)
		{
			//Player loses health according to the amount of damage specified
			playerHealth -= _damage;
			anim.SetBool ("Hit", true);
			hitTimer = hitMaxTimer;

			hitZoom.HitZoom ();
			hitScreen.gameObject.SetActive (true);
		}
	}

	public void healthRestore(float _restore)
	{
		playerHealth += _restore;
	}

	public void death()
	{
		playerHealth = 0f;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Checkpoint"))
		{
			CheckpointScript.checkpointPos = transform.position;
		}
	}
}
