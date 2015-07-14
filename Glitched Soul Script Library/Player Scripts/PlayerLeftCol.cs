using UnityEngine;
using System.Collections;

//Script Objective: Just left collider things....

/*Script Source:
 * How to set child object not to rotate to parent: http://answers.unity3d.com/questions/423031/how-do-i-set-a-child-object-to-not-rotate-if-the-p.html 
*/

public class PlayerLeftCol : MonoBehaviour 
{
	//Left Wall Detection
	static public bool leftWallDetect; 

	//Collision function to detect objects that touch Left Collider
	void OnTriggerStay (Collider other)
	{
		//If the Left Collider detects the environment
		if (other.gameObject.CompareTag("Environment"))
		{
			leftWallDetect = true; 
		}
	}
	
	//If the collider exits an object
	void OnTriggerExit(Collider other)
	{
		//If the left collider does not detect the environment
		if (other.gameObject.CompareTag("Environment"))
		{
			leftWallDetect = false; 
		}
	}
}
