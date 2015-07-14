using UnityEngine;
using System.Collections;

public class MetalGearFlamethrower : MonoBehaviour 
{
	//Bullet Variables
	public Transform bulletNode;
	public Transform flame;

	public float flameMaxTimer = 0.5f;
	private float flameTimer;
	private bool flameThrower;
	private bool damaging = false; 
	public float damage = 5f;

	public Transform warning;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(flameThrower == true)
		{
			flameTimer -= Time.deltaTime; 

			if(flameTimer <= 0f)
			{
				damaging = true;
			}
		}

	}

	//FLAMETHROWER IF PLAYER JUMPS IN FRONT
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			foreach (Transform child in bulletNode)
			{
				//Shoot bullet
				Transform flash = Instantiate (flame, child.position, child.rotation) as Transform;
				flash.transform.parent = transform;
			}

			flameTimer = flameMaxTimer;
			flameThrower = true;

			warning.gameObject.SetActive (true);
		}
	}


	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag ("Player") && damaging == true)
		{
			other.gameObject.SendMessage ("applyPlayerDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			damaging = false;
			flameThrower = false;
			flameTimer = flameMaxTimer;
			warning.gameObject.SetActive (false);
		}
	}
}
