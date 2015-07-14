using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour 
{
	public Transform player;
	
	public Transform bulletNode;
	public Transform bullet;
	public Transform muzzleFlash;
	public float bulletSpeed = 1000.0f;
	
	//Rapid Fire Variables
	public float fireRate = 0.1f;
	private float nextFire = 0.0f;

	LineRenderer lineRenderer;
	public int laserDist = 100;
	
	//Gun Line of sight
	[System.NonSerialized]	// Don't want to see in inspector
	public bool gunLos;
	public float gunMaxLos = 5f;
	[System.NonSerialized]	// Don't want to see in inspector
	public float gunLosTimer;

	//Corruption variables - when player is corrupting enemy
	public float corruptDist = 20f;
	public Transform corruptParticle;	

	public Transform patrolAim;
	
	
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		//when player is spotted, shoot at him in bursts
		if(gunLos && !PlayerUpgrade.corrupt && player)
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
		
		//Create a line renderer component for the laser
		lineRenderer = gameObject.GetComponent<LineRenderer>();

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
			
			if(hit.transform)
			{				
				if(hit.transform.CompareTag("Player"))
				{
					gunLosTimer = gunMaxLos;
					gunLos = true;
					//Notify Music to turn into combat
					GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
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



		//if the player is detected and still alive the turret should look at player continously and also not corrupted
		if(gunLos && player)
		{
			if(PlayerUpgrade.corrupt == false)
			{
				//Disable corrupt particle
				corruptParticle.gameObject.SetActive (false);
				//Aim at player
				Vector3 relativePos = player.position - transform.position;
				Quaternion rotation = Quaternion.LookRotation (relativePos);
				transform.rotation = rotation;	
			}
		}
		else
		{
			if(PlayerUpgrade.corrupt == false)
			{
				//Disable corrupt particle
				corruptParticle.gameObject.SetActive (false);

				//Patrol aim
				Vector3 relativePos = patrolAim.position - transform.position;
				Quaternion rotation = Quaternion.LookRotation (relativePos);
				transform.rotation = rotation;	
			}
		}

		if(player && PlayerUpgrade.corrupt == true)
		{
			//calculate distance of player
			float playerDist = Vector3.Distance(player.position, transform.position);
			
			if(playerDist <= corruptDist)
			{
				corruptParticle.gameObject.SetActive (true);
			}
		}
		
		if(gunLosTimer <= 0f)
		{
			gunLos = false;
		}
	}
}
