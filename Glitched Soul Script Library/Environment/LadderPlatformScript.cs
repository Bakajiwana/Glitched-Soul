using UnityEngine;
using System.Collections;

//Script Objective: when the player exits on top of the ladder he should be able to stand and not fall back down to the bottom.

public class LadderPlatformScript : MonoBehaviour 
{
	//Collider transform
	public Transform platform;

	private bool platformOpen;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.S) && platformOpen || PlayerLadderClimb.ladderClimb)
		{
			platform.gameObject.SetActive (false);
		}
		else
		{
			platform.gameObject.SetActive (true);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			platformOpen = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			platformOpen = false;
		}
	}
}
