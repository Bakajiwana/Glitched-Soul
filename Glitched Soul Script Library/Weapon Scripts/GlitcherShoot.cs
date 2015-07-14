using UnityEngine;
using System.Collections;

//Script objective: the shooty shoot for the shooty gun that is The GLITCHER Assault Rifle

/* PROBLEM FACED
 * Enumerator stops fucking working when the gun is set not active. 
*/ 

public class GlitcherShoot : MonoBehaviour 
{
	//Variables go here bro
	//Bullet Variables
	public Transform bulletNode;
	public Transform bullet;
	public Transform muzzleFlash;
	public float bulletSpeed = 3000.0f;

	//Rapid Fire Variables
	public float fireRate = 0.1f;
	private float nextFire = 0.0f;

	//overheat variables
	public Transform overHeatSteam;


	// Update is called once per frame
	void Update () 
	{
		//Create the fire button using the left mouse button where the player shoots bullets
		if(!GlitcherOverheat.overHeat && PlayerPause.paused == false)
		{
			if(Input.GetButton ("Fire1") && Time.time > nextFire)
			{
				//Rapid firing time
				nextFire = Time.time + fireRate;
				//Loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in bulletNode)
				{
					//Shoot bullet
					Transform shoot = Instantiate (bullet, child.position, child.rotation) as Transform;
					shoot.rigidbody.AddForce (transform.right * bulletSpeed);
					//Create Muzzle flash
					Transform flash = Instantiate (muzzleFlash, child.position, child.rotation) as Transform;
					flash.transform.parent = transform;
					//For every bullet that fires increase heat
					GlitcherOverheat.heat++;
				}
			}
		}

		//The reason for this bit is to instantiate a over heat steam once, as the child of the gun.
		if(GlitcherOverheat.heat >= GlitcherOverheat.overHeatCoolDown)
		{
			//Instantiate a steam feedback as a child
			Transform steam = Instantiate (overHeatSteam, transform.position, transform.rotation) as Transform;
			steam.transform.parent = transform;
		}
	}
}
