using UnityEngine;
using System.Collections;

/*SCRIPT OBJECTIVE
 	This script is in charge of controlling the primary movement of the character. In addition this script will use the 
	mecanim's variables to control the animations from the movement of this script.
*/

/* SOURCE CODE: (Tweaked...)
 * www.digitaltutors.com - Introduction to Mecanim (playerCont Script)
 * http://answers.unity3d.com/questions/164218/c-changing-the-radius-of-the-sphere-collider.html - Changing Collider Size
 * Crouch was first placed in void FixedUpdate, this caused unresponsive behaviour. Moved to void Update for more accuracy. 
*/

/*PROBLEMS EXPERIENCED:
 * The Left Control key stops the other keys on the keyboard from working, in other words you can't use left ctrl with any other button. Crouch button moved to Left Shift. 
*/

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (Rigidbody))]


public class PlayerController: MonoBehaviour 
{
	//Variables go here:
	
	private float m_moveSpeed; 	//Mesh Movement Speed
	public float m_runSpeed = 10.0f;
	public float m_crouchSpeed = 5.0f;

	private bool crouchZone = false;
	
	public float anim_speed = 1f;		//Sets the speed of all the animations being played

	public float blastMaxTimer = 0.1f;
	private float blastTimer;

	private Animator anim;				//A variable reference to the animator of the character

	//Crouch Collider Variables (These variables were measured from the scene and obtained from the capsule collider)
	private float crouchColliderHeight = 2.45f;
	private Vector3 crouchColliderCenter = new Vector3 (0.61f, -0.83f, 0f);
	private float standColliderHeight = 4f;
	private Vector3 standColliderCenter = new Vector3 (0f, 0f, 0f);
	private CapsuleCollider myCollider;	//Capsule Collider Variable 

	public float speedSpeed = 15f;


	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		//Obtain the transform information of the Capsule Collider
		myCollider = transform.GetComponent<CapsuleCollider>();

		//Set movement speed
		m_moveSpeed = m_runSpeed;
	}

	void OnAnimatorMove() //Tells Unity that root motion is handled by the script
	{
		if(anim)
		{
			Vector2 newPosition = transform.position;
			newPosition.x += anim.GetFloat("Direction") * m_moveSpeed * Time.deltaTime; 
			transform.position = newPosition; 	//Move the object smoothly with the Mecanim Direction variable
		}
	}

	void Update ()
	{
		//If the player's movement is disabled
		if (PlayerLadderClimb.ladderClimb)
		{
			m_moveSpeed = 0f; //Disable movement
		}
		else
		{
			if(PlayerUpgrade.speed)
			{
				m_moveSpeed = speedSpeed;
			}
			else
			{
				m_moveSpeed = m_runSpeed; //restore default speed
			}
		}

		if(crouchZone == true)
		{
			m_moveSpeed = m_crouchSpeed;
		}

		//CROUCH STATE
		//If the player holds left shift key is pressed
		if(Input.GetKey(KeyCode.LeftShift) && PlayerLadderClimb.ladderClimb == false)
		{
			anim.SetBool ("Crouch", true); 		//Set the crouch boolean to true

			//Change the capsule collider according to crouch size
			myCollider.height = crouchColliderHeight;
			if(anim.GetBool ("Grounded") == true)
			{
				myCollider.center = crouchColliderCenter;
			}
			if(anim.GetBool ("Grounded") == false)
			{
				myCollider.center = standColliderCenter;
			}

			//Decrease the speed only if player is grounded
			if(PlayerGrounded.groundDetect == true)
			{
				m_moveSpeed = m_crouchSpeed;				//and Decrease the speed of the player because he is crouched

			}
			else
			{
				if(PlayerUpgrade.speed)
				{
					m_moveSpeed = speedSpeed;
				}
				else
				{
					m_moveSpeed = m_runSpeed; //restore default speed
				}
			}
		}	
		
		if(Input.GetKeyUp(KeyCode.LeftShift) && crouchZone == false)
		{
			anim.SetBool ("Crouch", false); 	//Set the crouch boolean to false

			if(PlayerUpgrade.speed)
			{
				m_moveSpeed = speedSpeed;
			}
			else
			{
				m_moveSpeed = m_runSpeed; //restore default speed
			}
			
			//Change the capsule collider according to stand size
			myCollider.height = standColliderHeight;			
			myCollider.center = standColliderCenter;			
	
		}

		//IF THE PLAYER IS SHOOTING
		if(Input.GetButton ("Fire1"))
		{
			anim.SetBool ("Shoot", true);
		}
		if(Input.GetButtonUp ("Fire1"))
		{
			anim.SetBool ("Shoot", false);
		}

		if(Input.GetButtonDown ("Fire2") && WeaponUpgrade.stopBlast == false)
		{
			anim.SetBool ("Blast", true);
			blastTimer = blastMaxTimer;
		}

		if(blastTimer >= 0f)
		{
			blastTimer -= Time.deltaTime;
			anim.SetBool ("Blast", true);
		}
		else
		{
			anim.SetBool ("Blast", false);
		}
	}

	
	// This function is called every fixed framerate frame
	void FixedUpdate () 
	{
		float h = Input.GetAxis("Horizontal");	//Create a variable to obtain horizontal input axis information
		anim.SetFloat ("Direction", h);			//Obtain the Mecanim Animator variable and set it to equal the horizontal input axis
		anim.speed = anim_speed; 				//Set the speed of our animator to the speed of anim_speed variable.


		//When Wall colliders hit make sure player stops moving further. 
		if(PlayerLeftCol.leftWallDetect == true)
		{
			if(anim.GetBool ("Invert") == false)
			{
				if(anim.GetFloat ("Direction") < 0)
				{
					anim.SetFloat ("Direction", 0); 
				}
			}
			else if(anim.GetBool ("Invert") == true)
			{
				if(anim.GetFloat ("Direction") > 0)
				{
					anim.SetFloat ("Direction", 0); 
				}
			}
		}

		//When Wall colliders hit make sure player stops moving further. 
		if(PlayerRightCol.rightWallDetect == true)
		{
			if(anim.GetBool ("Invert") == false)
			{
				if(anim.GetFloat ("Direction") > 0)
				{
					anim.SetFloat ("Direction", 0); 
				}
			}
			else if(anim.GetBool ("Invert") == true)
			{
				if(anim.GetFloat ("Direction") < 0)
				{
					anim.SetFloat ("Direction", 0); 
				}
			}
		}
	}

	//Crouch only zones
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag("Crouch Zone"))
		{
			crouchZone = true;
		}
	}


	void OnTriggerExit(Collider other)
	{
		//If the ground detector does not detect the environment
		if (other.gameObject.CompareTag("Crouch Zone"))
		{
			crouchZone = false;
			if(!Input.GetKey (KeyCode.LeftShift))
			{
				anim.SetBool ("Crouch", false);

				if(PlayerUpgrade.speed)
				{
					m_moveSpeed = speedSpeed;
				}
				else
				{
					m_moveSpeed = m_runSpeed; //restore default speed
				}
				
				//Change the capsule collider according to stand size
				myCollider.height = standColliderHeight;
				myCollider.center = standColliderCenter;
			}
		}
	}
}
	

