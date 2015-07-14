using UnityEngine;
using System.Collections;

public class TurretLookAt : MonoBehaviour 
{
	//Destination variables
	public Transform[] wayPoint; 
	private int currentWayPoint = 0;
	
	public float speed = 10f;

	//Idle Variables
	private bool idle;
	public float idleMaxTimer = 2f;
	private float idleTimer;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!idle) //if not idle then move to next point
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, wayPoint[currentWayPoint].position, step); 
		}
		else //pause for a bit 
		{
			if(idleTimer >= 0f)
			{
				idle = true;
				idleTimer -= Time.deltaTime;
			}
			else
			{
				idle = false;
			}
		}
	}


	//This function is called when the destination cube needs to switch to a new waypoint.
	void SwitchWaypoint()
	{
		//Increment the current way point so the array of the wayPoint variable will move up
		if(currentWayPoint < wayPoint.Length)
		{
			currentWayPoint ++;
		}
		
		if(currentWayPoint == wayPoint.Length)
		{
			currentWayPoint = 0;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		//If enemy collides with a waypoint and threat level is at 1 then go straight to idle state
		if(other.gameObject.CompareTag ("Enemy Waypoint"))
		{
			//Switch way point locations
			SwitchWaypoint ();
			//Reset idle timer
			idleTimer = idleMaxTimer;
			//Idle is set to true to stop the patrol function
			idle = true;
		}
	}
}
