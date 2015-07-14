using UnityEngine;
using System.Collections;

//SCRIPT OBJECTIVE: To Climb or not to climb, that is the question?

public class PlayerLadderClimb : MonoBehaviour {

	//Ladderclimb Variables
	public float m_climbSpeed = 10f; 
	public static bool ladderClimb = false;

	//Rotation Variables
	private float ladderSide = -90f; 	//This variable represents the rotation value to face the ladder
	
	private Animator anim;				//A variable reference to the animator of the character

	public float anim_speed = 1f;		//Sets the speed of all the animations being played

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}

	void OnAnimatorMove() //Tells Unity that root motion is handled by the script
	{
		if(anim)
		{
			Vector3 newPosition = transform.position;
			newPosition.y += anim.GetFloat("Climb Speed") * m_climbSpeed * Time.deltaTime; 
			transform.position = newPosition; 	//Move the object smoothly with the Mecanim Direction variable
		}
	}


	// This function is called every fixed framerate frame
	void Update () 
	{
		//If within a ladder trigger than allow climb function
		if (ladderClimb == true)
		{
			Climb(); //activate the climb function
			transform.eulerAngles = new Vector3(0f, ladderSide, 0f); //look at the ladder
			if(anim.layerCount >=2)
			{
				anim.SetLayerWeight(1, 0); //Decrease weight on the override layer so both arms are free
			}
		}


		else
		{
			if(anim.layerCount >=2)
			{
				anim.SetLayerWeight(1, 1); //Increase weight on Aim Layer to regain aiming
			}
			anim.SetFloat ("Climb Speed", 0f); //Prevents the animation from getting stuck as soon as the player leaves collider
		}
	}

	//As long as the player is within the ladder collider
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.CompareTag("Ladder"))
		{
			if(Input.GetButtonDown ("Vertical")) //Activate the ladder climb functions when vertical buttons are pressed in the collider
			{
				ladderClimb = true; 	//Activate the ladder boolean, stating that the player can climb on the ladders
				anim.SetBool ("Ladder Climb", ladderClimb); //Set the mecanim variable to the ladder climb boolean variable
			}

			//If the player is going down the ladder and reaches the floor then turn off ladderClimb
			float land = Input.GetAxis ("Vertical");
			if (land < 0.01 && PlayerGrounded.groundDetect == true)
			{
				ladderClimb = false; //turns off climbing boolean
				anim.SetBool ("Ladder Climb", false); //turn off mecanim climbing animation
				rigidbody.useGravity = true; //Reset gravity so player doesn't blast away
			}
		}
	}

	//Collision function to detect ladder triggers
	void OnTriggerExit (Collider other)
	{
		//If the player runs into a ladder trigger
		if (other.gameObject.CompareTag("Ladder"))
		{
			ladderClimb = false; 	//Activate the ladder boolean, stating that the player can climb on the ladders
			anim.SetBool ("Ladder Climb", ladderClimb); //Set the mecanim variable to the ladder climb boolean variable

			rigidbody.useGravity = true; //When player leaves the collider turn gravity back on
		}
	}

	//Ladder climb function
	void Climb()
	{
		float v = Input.GetAxis("Vertical");	//Create a variable to obtain vertical input axis information
		anim.SetFloat ("Climb Speed", v);		//Obtain the Mecanim Animator variable and set it to equal the vertical input axis
		anim.speed = anim_speed; 				//Set the speed of our animator to the speed of anim_speed variable.

		rigidbody.useGravity = false; //Turn off gravity to stop falling
	}
}
