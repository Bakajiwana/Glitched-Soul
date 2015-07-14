using UnityEngine;
using System.Collections;

//The particle explosion in level 3

public class level3Explosion : MonoBehaviour 
{

	public Transform particleExplosion;

	public void DataOverload()
	{
		Instantiate (particleExplosion, transform.position, transform.rotation);
	}
}
