using UnityEngine;
using System.Collections;

//OBJECTIVE: This script handles the rotation and aiming of the player.

/*SOURCE CODE: (Tweaked and studied of course....)
 * http://answers.unity3d.com/questions/506810/2d-shooter-plataform-aimtutorial-please.html - Understanding 2D Aiming
 * http://gamedev.stackexchange.com/questions/14602/what-are-atan-and-atan2-used-for-in-games - Understanding trig in games
 * http://docs.unity3d.com/Documentation/ScriptReference/Mathf.Atan.html
*/

public class PlayerRotation : MonoBehaviour 
{
	//Mouse Position Variables
	private Vector3 mousePos;
	public Transform player;

	//Object Position Variables
	private Vector3 playerPos;

	//Access Mecanim Variables
	private Animator anim;

	//Angle variable
	private float angle;

	//Player Orientation Variables
	private float aimLeftSide = 180f;
	private float aimRightSide = 0f;

	private bool invertAim;	//False if look at the original side (left) and True if looking at the left side.

	//Invert wall climb rotation variables
	private bool invertClimbLeft;
	private bool invertClimbRight;

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Obtain the mouse position
		mousePos = Input.mousePosition;
		mousePos.z = 0; //Z mouse coordinate is not necessary for 2D game space and may effect rotation

		//Obtain the player position using the camera
		playerPos = Camera.main.WorldToScreenPoint(player.position);

		//Obtain the difference between the mouse and player coordinates
		mousePos.x = mousePos.x - playerPos.x;
		mousePos.y = mousePos.y - playerPos.y; 

		//Use the difference of x/y to find the angle using Atan2(x/y). 
		angle = Mathf.Atan2 (mousePos.x, mousePos.y) * Mathf.Rad2Deg; //This will return aiming coordinates
	
		//Player Orientation
		//Override from invert wall climb (condition: when mouse position is in negative space)
		if(PlayerLadderClimb.ladderClimb == false)
		{
			//When player's mouse aims to the left side of the screen the player rotation will aim left and vice versa
			if(mousePos.x > 0) //If mouse is on the left side of the player
			{
				transform.eulerAngles = new Vector3(0f, aimRightSide, 0f); //Look at the right side
				invertAim = false; //Aiming at original position (right side)
				anim.SetBool ("Invert", invertAim);
			}

			if(mousePos.x < 0) //If mouse is on the right side of the player
			{
				transform.eulerAngles = new Vector3(0f, aimLeftSide, 0f); //look at the left side
				invertAim = true; //Aiming at opposite side (left side)
				anim.SetBool ("Invert", invertAim);
			}
		}
		anim.SetFloat ("AimCoord", angle);	//Send information to animator for aiming animations
	}
}
