using UnityEngine;
using System.Collections;

//Script Objective when the player is against a wall.

/*References:
 * rigidbody decelaration: http://answers.unity3d.com/questions/65021/how-i-can-slow-down-a-rigidbody-little-by-little.html
 * http://answers.unity3d.com/questions/512650/how-can-i-slow-stop-the-rigidbody-speed.html
 */ 

public class PlayerWallClimb : MonoBehaviour 
{
	//Wall Slide variables
	public float m_slideSpeed = 0.5f;

	//Wall Jump Variables
	public float wallLaunch;
	public float wallJump;

	private Animator anim;				//A variable reference to the animator of the character

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}

		
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//If the left and right colliders, collide into a wall make the player slide down
		if(PlayerLeftCol.leftWallDetect == true && PlayerGrounded.groundDetect == false)
		{
			anim.SetBool ("Wall Climb", true);

			//Slows down rigidbody over time. NOTE: m_slideSpeed value must be less than 1.0f...
			rigidbody.velocity = rigidbody.velocity * m_slideSpeed;

		}
		else 
		{
			anim.SetBool ("Wall Climb", false);
		}

		//If the left and right colliders, collide into a wall make the player slide down
		if(PlayerRightCol.rightWallDetect == true && PlayerGrounded.groundDetect == false)
		{
			anim.SetBool ("Invert Wall Climb", true);
			
			//Slows down rigidbody over time. NOTE: m_slideSpeed value must be less than 1.0f...
			rigidbody.velocity = rigidbody.velocity * m_slideSpeed;
			
		}
		else 
		{
			anim.SetBool ("Invert Wall Climb", false);
		}
	}

	void Update()
	{
		//WALL JUMP
		//If wall climb animation is true and spacebar is hit = wall jump
		if(anim.GetBool ("Wall Climb") == true && Input.GetKeyDown (KeyCode.Space))
		{
			//use physics to launch player
			rigidbody.AddForce (Vector3.up*wallJump);	//Launch upwards

			if(anim.GetBool ("Invert") == true)
			{
				rigidbody.AddForce (Vector3.left*wallLaunch);	//Launch Right
			}
			else
			{
				rigidbody.AddForce (Vector3.right*wallLaunch);		//Launch Left
			}
		}

		if(anim.GetBool ("Invert Wall Climb") == true && Input.GetKeyDown (KeyCode.Space))
		{
			//use physics to launch player
			rigidbody.AddForce (Vector3.up*wallJump);	//Launch upwards
			
			if(anim.GetBool ("Invert") == true)
			{
				rigidbody.AddForce (Vector3.right*wallLaunch);	//Launch Right
			}
			else
			{
				rigidbody.AddForce (Vector3.left*wallLaunch);		//Launch Left
			}
		}
	}
}
