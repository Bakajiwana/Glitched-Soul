using UnityEngine;
using System.Collections;

//Script Objective: Turn off player rigidbody gravity during a jump pad area
public class PlayerJumpPad : MonoBehaviour 
{
	static public bool inJumpZone = false;

	void Update()
	{
		//If in jump pad, the player should be able to activate it and move up
		if(inJumpZone == true && Input.GetKeyDown (KeyCode.Space))
		{
			rigidbody.useGravity = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Jump Zone"))
		{
			inJumpZone = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.gameObject.CompareTag ("Jump Zone"))
		{
			rigidbody.useGravity = true;
			inJumpZone = false;
		}
	}
}
