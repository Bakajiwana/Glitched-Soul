using UnityEngine;
using System.Collections;

//Script Objective: Make the enemy follow a destination using nav mesh agent.

//Reference: Digital Tutors New Features in Unity 3.5: http://www.digitaltutors.com/tutorial/693-New-Features-in-Unity-3.5#play-14898

public class DestinationObject : MonoBehaviour 
{
	//Nav mesh variables
	public NavMeshAgent agent;

	// Use this for initialization
	void Start () 
	{
		agent.destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Find location of agent and move to it.
		CheckLocation();
	}

	//Find location of the nav mesh agent
	void CheckLocation()
	{
		//make the agents position equal the enemy's position
		agent.destination = transform.position;
	}
}
