using UnityEngine;
using System.Collections;

//If player takes damage then the camera will zoom.
//NOTE: THIS SCRIPT IS ATTACHED TO THE PARENT OF THE CAMERA TO CONTROL THE ANIMATION

public class CameraHitZoom : MonoBehaviour 
{
	private Animator anim;				//A variable reference to the animator of the character

	void Start()
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}

	public void HitZoom()
	{
		anim.SetTrigger ("zoom");
	}
}
