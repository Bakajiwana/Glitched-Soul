using UnityEngine;
using System.Collections;

//This script is to emit a particle, when object collides with anything
//Mainly used for the sword

public class particleEmitter : MonoBehaviour 
{

	public Transform spark;
	public Transform blood;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Enemy"))
		{
			if(blood)
			{
				Instantiate (blood, transform.position, transform.rotation);
			}
		}

		if(other.gameObject.CompareTag("Environment"))
		{
			if(spark)
			{
				Instantiate (spark, transform.position, transform.rotation);
			}
		}
	}
}
