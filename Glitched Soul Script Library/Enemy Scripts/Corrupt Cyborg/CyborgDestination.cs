﻿using UnityEngine;
using System.Collections;

//Script Objective: Make the enemy follow a destination using nav mesh agent.

//Reference: Digital Tutors New Features in Unity 3.5: http://www.digitaltutors.com/tutorial/693-New-Features-in-Unity-3.5#play-14898

public class CyborgDestination: MonoBehaviour 
{
	//Nav mesh variables
	public NavMeshAgent agent;
	public int walkSpeed = 8;
	public int runSpeed = 14;
	public int meleeSpeed = 7;
	public int staggerSpeed = 3;

	public CyborgMeleeScript cyborgScript;	//Access variables from main script

	public CyborgHealth cyborgHealth;	//Access variables from health script
	
	// Use this for initialization
	void Start () 
	{
		agent.destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Find location of agent
		CheckLocation();

		//obtain location information from the agent
		transform.position = cyborgScript.destination.position;

		//If the enemy detected player, increase nav speed, BUT THAT'S ONLY IF HE IS NOT STAGGERED
		if(cyborgHealth.stagger == false)
		{
			//If the player is not detected, then walk
			if(cyborgScript.detected == false)
			{
				agent.speed = walkSpeed;
			}
			else //if the player is detected then run, but during melee speed is decreased
			{
				if(cyborgScript.melee == false)
				{
					agent.speed = runSpeed;
				}
				else
				{
					agent.speed = meleeSpeed;
				}
			}
		}
		else //the speed is slowed down due to stagger effect
		{
			agent.speed = staggerSpeed;
		}
	}
	
	//Find location of the nav mesh agent
	void CheckLocation()
	{
		//make the agents position equal the enemy's position
		agent.destination = transform.position;
	}
}
