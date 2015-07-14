using UnityEngine;
using System.Collections;

public class PlayerLaserSight : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//Laser Sight should be true
		transform.renderer.enabled = true;

		//No need for the mouse cursor
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Toggle laser
		if(Input.GetKeyDown (KeyCode.Q))
		{
			if(transform.renderer.enabled == false)
			{
				transform.renderer.enabled = true;
			}
			else
			{
				transform.renderer.enabled = false;
			}
		}
	}
}
