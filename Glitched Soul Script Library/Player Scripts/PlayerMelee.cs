using UnityEngine;
using System.Collections;

//SCRIPT OBJECTIVE: The Melee Functions of the player.

/*Sources:
 * How to implement a directional melee attack: http://gamedev.stackexchange.com/questions/69344/how-can-i-implement-a-directional-melee-attack
 * Melee Damage and collisions: http://answers.unity3d.com/questions/478588/melee-damage-script-by-collision.html
*/

/*Plan:
 * I will need to put animations into mecanim 
 * create an Integer instead of a boolean to count melee and execute
 * when player hits button, increment Integer and start combo
 * start a combo timer to count down for the player to repeat before timer runs out else start combo float back to 0
 * Disable movement controls
*/

/*Major Problems
 * Collider when disabling PlayerControl. It just affects that one line. that stops crouch colliders 
*/ 
public class PlayerMelee : MonoBehaviour 
{
	//Melee Variables
	public int meleeCombo = 0;
	private int meleeReset = 0;
	private float meleeTimer; 
	public float meleeTimerReset = 1f;
	public float meleeTimerSpan = 0.25f;
	private bool block = false; 
	public Transform shield;
	private bool meleeUppercut = false; 

	//Mecanim variables
	private Animator anim;				//A variable reference to the animator of the character

	//Script Variables grabbing these scripts to disable them during melee
	public PlayerLadderClimb ladderClimb;
	public PlayerController controller;
	public PlayerJump jump;

	private CapsuleCollider myCollider;	//Capsule Collider Variable 

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		//Initialise timer
		meleeTimer = meleeTimerReset;

		//Obtain the transform information of the Capsule Collider
		myCollider = transform.GetComponent<CapsuleCollider>();

		//Get Script components
		ladderClimb = GetComponent<PlayerLadderClimb>();
		controller = GetComponent<PlayerController>();
		jump = GetComponent<PlayerJump>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//STANDARD MELEE
		//If the melee combo is at 0 (which is no melee) and make sure other melee functions don't interfere
		if(meleeCombo == 0 && block == false && meleeUppercut == false && PlayerGrounded.groundDetect == true)
		{
			//If the f key is pressed activate the melee function
			if(Input.GetKeyDown(KeyCode.F))
			{
				Melee();
			}
		}

		//If any standard melee states are on and make sure other melee functions don't interfere
		if(meleeCombo == 1 || meleeCombo == 2 || meleeCombo == 3  && block == false && meleeUppercut == false && PlayerGrounded.groundDetect == true)
		{
			meleeTimer -= Time.deltaTime;	//Start the timer

			if(anim.layerCount >=2 && PlayerGrounded.groundDetect == true)
			{
				anim.SetLayerWeight(1, 0); //Decrease weight on the override layer so both arms are free
			}

			//While the timer is decreasing the player will have a limited time to press f again. If player presses button within time span.
			if(Input.GetKeyDown(KeyCode.F) && meleeTimer <= meleeTimerSpan && PlayerGrounded.groundDetect == true)
			{
				//If meleeCombo equals 3 which is the last standard melee state, reset the count.
				if(meleeCombo == 3)
				{
					MeleeReset ();
				}

				//Activate melee function to create phase to phase melee.
				Melee ();
			}

			//If the player doesn't press f again within the timespan the meleeCombo and timer will be reset
			if(meleeTimer <= 0)
			{
				MeleeReset ();
			}
		}


		// MELEE UPPERCUT
		//If the left shift button and f key are pressed, activate uppercut melee
		if(Input.GetKeyDown(KeyCode.F) && Input.GetKey (KeyCode.LeftShift))
		{
			meleeUppercut = true;
			anim.SetBool ("Melee Uppercut", meleeUppercut);
			meleeTimer = meleeTimerReset;					//reset timer
			meleeCombo = 0;
		}

		//While the meleeUppercut boolean is true, start a time and disable weights.
		if(meleeUppercut && meleeCombo == 0 && block == false)
		{
			meleeTimer -= Time.deltaTime;

			if(anim.layerCount >=2)
			{
				anim.SetLayerWeight(1, 0); //Decrease weight on the override layer so both arms are free
			}

			if(meleeTimer <= 0.0f)
			{
				MeleeReset ();

				if(anim.layerCount >=2)
				{
					anim.SetLayerWeight(1, 1); //Increase weight on Aim Layer to regain aiming
				}

				anim.SetBool ("Crouch", false); 	//Set the crouch boolean to false
				myCollider.center = Vector3.zero;

				meleeUppercut = false;
				anim.SetBool ("Melee Uppercut", meleeUppercut);
			}
		}


		//BLOCK
		//If Left alt is held
		if(Input.GetKey (KeyCode.LeftAlt) && meleeUppercut == false && meleeCombo == 0)
		{
			block = true;
			anim.SetBool ("Block", block);

			if(anim.layerCount >=2)
			{
				anim.SetLayerWeight(1, 0); //Decrease weight on the override layer so both arms are free
			}

			//If the players shield is over 0 then shield should protect player from where he is facing
			if(PlayerShield.playerShield > 0f)
			{
				shield.gameObject.SetActive (true);
			}
			else
			{
				shield.gameObject.SetActive (false);
			}

			//Turn off Scripts to prevent moving, jumping and interference from the ladder script
			ladderClimb.enabled = false;
			controller.enabled = false; 
			jump.enabled = false;
		}

		//When left alt is let go
		if(Input.GetKeyUp (KeyCode.LeftAlt) && !Input.GetKey (KeyCode.LeftShift))
		{
			block = false;
			anim.SetBool ("Block", block);

			if(anim.layerCount >=2)
			{
				anim.SetLayerWeight(1, 1); //Increase weight on Aim Layer to regain aiming
			}

			//The player is no longer protected
			shield.gameObject.SetActive (false);
			
			//Turn on scripts
			ladderClimb.enabled = true; 
			controller.enabled = true; 
			jump.enabled = true;
		}
	}

	void Melee()
	{
		meleeCombo++;									//Increase meleeCombo by 1
		anim.SetInteger ("Melee Combo", meleeCombo);	//Set the integer of meleeCombo into mecanim
		meleeTimer = meleeTimerReset;					//reset timer

		//Turn off Scripts to prevent moving, jumping and interference from the ladder script
		ladderClimb.enabled = false;
		controller.enabled = false; 
		jump.enabled = false;
	}

	void MeleeReset()
	{
		meleeTimer = meleeTimerReset;					//Reset timer
		meleeCombo = meleeReset; 						//Reset combos
		anim.SetInteger ("Melee Combo", meleeCombo);	//Update mecanim animation

		if(anim.layerCount >=2)
		{
			anim.SetLayerWeight(1, 1); //Increase weight on Aim Layer to regain aiming
		}
		
		//Turn on scripts
		ladderClimb.enabled = true; 
		controller.enabled = true; 
		jump.enabled = true;
	}
}
