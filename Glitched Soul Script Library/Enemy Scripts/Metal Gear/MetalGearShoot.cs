using UnityEngine;
using System.Collections;

public class MetalGearShoot : MonoBehaviour 
{
	//Variables go here bro
	//Bullet Variables
	public Transform bulletNode;
	public Transform bullet;
	public Transform muzzleFlash;
	public float bulletSpeed = 2000.0f;
	
	//Rapid Fire Variables
	public float fireRate = 0.1f;
	private float nextFire = 0.0f;

	public void Fire()
	{
		if(Time.time > nextFire)
		{
			//Rapid firing time
			nextFire = Time.time + fireRate;
			//Loop through the children of the bulletnode and create a bullet for each node
			foreach (Transform child in bulletNode)
			{
				//Shoot bullet
				Transform shoot = Instantiate (bullet, child.position, child.rotation) as Transform;
				shoot.rigidbody.AddForce (transform.forward * bulletSpeed);
				//Create Muzzle flash
				Transform flash = Instantiate (muzzleFlash, child.position, child.rotation) as Transform;
				flash.transform.parent = transform;
			}
		}
	}
}
