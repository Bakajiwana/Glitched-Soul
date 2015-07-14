using UnityEngine;
using System.Collections;

//Script Objective: Start screen tasks, to zoom in after key is pressed

//Script Source: How to detect if any key has been pressed down: http://answers.unity3d.com/questions/23443/how-to-detect-if-any-key-has-been-pressed-down.html
public class StartScript : MonoBehaviour 
{
	//Main Menu variables
	private bool mainMenu = false;

	//Destination variable to move the camera
	public Transform endPoint;

	//Disable the title screen when the player presses a key to move on to the main menu
	public Transform startScreen;

	//Activate the main menu when the player presses a key in the title screen
	public Transform mainScreen;

	//Camera speed during transition
	public float time = 5f;
	private float elapsedTime = 0f;

	//Script reference to activate when in main menu
	public MouseLook mouseAim;

	// Use this for initialization
	void Start () 
	{
		//Get Script components
		mouseAim = GetComponent<MouseLook>();
		//On start screen the MouseLook script shouldn't be on
		mouseAim.enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		//when main menu is turned on
		if(mainMenu == true)
		{
			//Disable start screen
			startScreen.gameObject.SetActive (false);

			//Activate main menu
			mainScreen.gameObject.SetActive (true);

			//Enable MouseLook script
			mouseAim.enabled = true;
		
			//move towards the end point (camera movement phase)
			//and Create local variable for movement
			//While the elapsed time is less than time
			if (elapsedTime < time)
			{
				Vector3 startArea = transform.position;
				Vector3 destination = endPoint.position;
				//Move between the two points
				transform.position = Vector3.Lerp (startArea, destination, (elapsedTime / time));
				elapsedTime += Time.deltaTime; //move to end point or MAIN MENU
			}
		}
	}

	void OnGUI()
	{
		if(mainMenu == false)
		{
			//If any key is pressed
			if(Event.current.type == EventType.KeyDown || Input.GetButton ("Fire1")|| Input.GetButton ("Fire2"))
			{
				KeyPressedEventHandler();
			}
		}
	}

	//Do anything when a key is pressed
	private void KeyPressedEventHandler()
	{
		//This is to stop the player from pressing any more buttons and disrupting this function
		mainMenu = true;
	}
}
