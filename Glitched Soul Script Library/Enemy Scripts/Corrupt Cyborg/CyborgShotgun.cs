using UnityEngine;
using System.Collections;

public class CyborgShotgun : MonoBehaviour {

	//Shooting variables
	public Transform bulletNode;
	public Transform bullet;
	public Transform muzzleFlash;
	public float bulletSpeed = 3000.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	public void Fire () 
	{
		foreach (Transform child in bulletNode)
		{
			//Shoot bullet
			Transform fire = Instantiate (bullet, child.position, child.rotation) as Transform;
			fire.rigidbody.AddForce (transform.right * bulletSpeed);
			//Create Muzzle flash
			Transform flash = Instantiate (muzzleFlash, child.position, child.rotation) as Transform;
			flash.transform.parent = transform;
		}	
	}
}
