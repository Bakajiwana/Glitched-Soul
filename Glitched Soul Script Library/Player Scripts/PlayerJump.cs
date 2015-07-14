using UnityEngine;
using System.Collections;

//SCRIPT OBJECTIVE: To jump or not to jump.

/*SOURCE CODE: (Tweaked...)
 * www.digitaltutors.com - Introduction to C#: Lesson_15_Movement.cs
 * http://answers.unity3d.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html - Jumping script
*/

/*PROBLEMS:
 * A Major time consuming problem was trying to use a raycast to detect the ground. 
   For some reason if the player is at a certain height of Y = 4, the raycast would detect that
   the player is grounded. (Was not resolved had to find a work around).
*/


public class PlayerJump : MonoBehaviour 
{
	//Mecanim variables
	private Animator anim;			//A variable reference to the animator of the character

	//Jump Variables
	private float jumpSpeed = 10000.0f;
	public float normalJumpSpeed = 10000.0f; 
	public float ladderJumpSpeed = 2000.0f;

	//Grounded variables
	private bool grounded; 
	private float jumpStartTimer; //Used to prevent the grounded boolean from getting stuck on true.
	public float jumpStartMaxTimer = 1f;

	//Collider Variables
	public float jumpColliderHeight = 2f;
	private float standColliderHeight = 4f;
	private CapsuleCollider myCollider;	//Capsule Collider Variable 


	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		//Obtain the transform information of the Capsule Collider
		myCollider = transform.GetComponent<CapsuleCollider>();

		//The jump speed should be initialised with the normal jump speed
		jumpSpeed = normalJumpSpeed;
	}


	void Update () 
	{
		//JUMPING OFF A LADDER
		//If the player's movement is limited from climbing a ladder
		if (PlayerLadderClimb.ladderClimb == true)
		{
			jumpSpeed = ladderJumpSpeed; //Decrease jump speed depending on ladder jump speed
			grounded = true;	//Prevents a launchpad effect at the bottom of the ladder

			//if player wants to jump off ladder
			if(Input.GetKeyDown (KeyCode.Space))
			{
				ladderJump ();
			}
		}
		else
		{
			jumpSpeed = normalJumpSpeed; //restore default jump speed
		}
	

		//JUMPING OFF NORMALLY
		//If the player presses the jump button

		grounded = PlayerGrounded.groundDetect;		//Grab the grounded result from the ground detect script

		if(Input.GetKeyDown (KeyCode.Space) && grounded == true) //If space bar is pressed
		{
			jump(); //run the jump function
			jumpStartTimer = jumpStartMaxTimer; //Set timer
		}

		//If the player is grounded
		if(grounded == true && jumpStartTimer <= 0f)
		{
			anim.SetBool ("Grounded", grounded); // Set the grounded variable for mecanim

			//make sure that he is not in a crouch animation or during melee animation
			if(anim.GetBool ("Crouch") == false)
			{
			//Change the capsule collider according to stand size
			myCollider.height = standColliderHeight;
			}
		}

		if(grounded == false)
		{
			anim.SetBool ("Grounded", grounded); //If the player fell an edge or something play airbourne animation
		}


		//If the timer is greater than 0
		if(jumpStartTimer > 0f)
		{
			jumpStartTimer -= Time.deltaTime; //Count down
		}
	}


	//The jump function
	void jump()
	{
		rigidbody.AddForce (Vector3.up*jumpSpeed);	//Use physics to boost the player up
		grounded = false; 
		myCollider.height = jumpColliderHeight; //Change the capsule collider according to jump size
		anim.SetBool ("Grounded", grounded); //Set the grounded variable for mecanim
	}


	//The ladder jump function
	void ladderJump()
	{
		PlayerLadderClimb.ladderClimb = false; //turns off climbing boolean
		anim.SetBool ("Ladder Climb", false); //turn off mecanim climbing animation
		rigidbody.useGravity = true; //Reset gravity so player doesn't blast away

		rigidbody.AddForce (Vector3.up*jumpSpeed);	//Use physics to boost the player up
		grounded = false; 
		myCollider.height = jumpColliderHeight; //Change the capsule collider according to jump size
		anim.SetBool ("Grounded", grounded); //Set the grounded variable for mecanim
	}
}
