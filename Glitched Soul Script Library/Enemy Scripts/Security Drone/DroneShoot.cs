using UnityEngine;
using System.Collections;

public class DroneShoot : MonoBehaviour 
{
	public Transform player;

	public Transform bulletNode;
	public Transform bullet;
	public Transform muzzleFlash;
	public float bulletSpeed = 1000.0f;
	
	//Rapid Fire Variables
	public float fireRate = 0.1f;
	private float nextFire = 0.0f;

	public DronePatrol patrolScript;

	//Shoot pause variable
	private bool shootPause;
	private int burst = 0;
	public int maxBurst = 10;
	public float shootMaxTimer = 2f;
	private float shootTimer;
	
	public Transform spotLight;
	LineRenderer lineRenderer;
	public int laserDist = 100;

	//Gun Line of sight
	[System.NonSerialized]	// Don't want to see in inspector
	public bool gunLos;
	public float gunMaxLos = 5f;
	[System.NonSerialized]	// Don't want to see in inspector
	public float gunLosTimer;

	
	void Start()
	{
		shootTimer = shootMaxTimer;
	}

	// Update is called once per frame
	void Update () 
	{
		//when player is spotted, shoot at him in bursts
		if(patrolScript.attack && PlayerPause.paused == false && !shootPause && player && PlayerUpgrade.corrupt == false)
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
					//For every bullet that fires increase burst count
					burst++;
				}
			}
		}

		//Create a line renderer component for the laser
		lineRenderer = gameObject.GetComponent<LineRenderer>();

		//if the player is detected and still alive the turret should look at player continously and also not corrupted
		if(patrolScript.attack && player && PlayerUpgrade.corrupt == false)
		{
			//Aim at player
			Vector3 relativePos = player.position - transform.position;
			Quaternion rotation = Quaternion.LookRotation (relativePos);
			transform.rotation = rotation;

			//Turn on laser sight and turn off spotlight
			spotLight.gameObject.SetActive (false);


			lineRenderer.enabled = true;
			lineRenderer.useWorldSpace = true;
			lineRenderer.SetVertexCount(2);

			Ray ray = new Ray(bulletNode.position, transform.forward);
			RaycastHit hit;
			//set the line renderer position to be at the end of the gun
			lineRenderer.SetPosition(0, ray.origin);

			if(Physics.Raycast(ray, out hit, laserDist))
			{
				lineRenderer.SetPosition(1, hit.point);

				if(hit.rigidbody)
				{
					hit.rigidbody.AddForceAtPosition(transform.right * 3, hit.point);

					if(hit.rigidbody.CompareTag("Player"))
					{
						gunLosTimer = gunMaxLos;
						gunLos = true;
					}
					else
					{
						gunLosTimer -= Time.deltaTime; 						
					}
				}
				else
				{
					lineRenderer.SetPosition(1, ray.GetPoint(laserDist));
				}
			}
		}

		if(gunLosTimer <= 0f)
		{
			gunLos = false;
		}

		if(patrolScript.attack == false)
		{
			spotLight.gameObject.SetActive (true);


			lineRenderer.enabled = false;
		}

		//If the turret shot more than the burst amount then pause for a bit before shooting again
		if(burst > maxBurst)
		{
			shootTimer -= Time.deltaTime;
			shootPause = true;
		}

		//If the pause timer is finished time to reset the burst and fire again
		if(shootTimer <= 0f)
		{
			shootPause = false;
			burst = 0;
			shootTimer = shootMaxTimer;
		}
	}
}
