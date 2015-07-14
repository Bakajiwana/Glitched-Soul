using UnityEngine;
using System.Collections;

//Script Objective: The Object with this script can be destroyed e.g. breakable tiles

public class BreakableObject : MonoBehaviour 
{
	public Transform destroyParticles; 

	private float objectHealth;
	public float objectMaxHealth = 100f;


	// Use this for initialization
	void Start () 
	{
		//Initiate object health
		objectHealth = objectMaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If object health runs out, then destroy the object and spawn particles
		if(objectHealth <= 0f)
		{
			Instantiate (destroyParticles, transform.position, transform.rotation);
			objectHealth = objectMaxHealth;
			transform.gameObject.SetActive (false);
		}
	}

	//This function is called when object gets hit
	public void applyObjectDamage (float _damage)
	{
		objectHealth -= _damage; 
	}
}
