using UnityEngine;
using System.Collections;

//Script Objective: Just right collider things....

/*Script Source:
 * How to set child object not to rotate to parent: http://answers.unity3d.com/questions/423031/how-do-i-set-a-child-object-to-not-rotate-if-the-p.html 
*/

public class PlayerRightCol : MonoBehaviour 
{
	static public bool rightWallDetect;

	//Collision function to detect objects that touch right Collider
	void OnTriggerStay (Collider other)
	{
		//If the right Collider detects the environment
		if (other.gameObject.CompareTag("Environment"))
		{
			rightWallDetect = true; 
		}
	}
	
	//If the collider exits an object
	void OnTriggerExit(Collider other)
	{
		//If the right collider does not detect the environment
		if (other.gameObject.CompareTag("Environment"))
		{
			rightWallDetect = false; 
		}
	}
}