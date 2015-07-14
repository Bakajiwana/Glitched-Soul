using UnityEngine;
using System.Collections;
//Script Objective: The Main Script for The Glitcher Bringer Sword. Just give it a damage and whatever enemy it touches, damage them.

public class GlitchBringerScript : MonoBehaviour 
{
	public float damage = 35f;

	public float swingDelay = 0.3f;
	private float delay;
	public AudioClip swing;

	void Update()
	{
		//PROBLEM WITH RIGIDBODY MOVING THE SWORD
		//STAY IN POSITION!!! GOD!!!!
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler(0,0,0);

		if(Input.GetKeyDown (KeyCode.F) && delay <= 0f)
		{
			audio.PlayOneShot (swing, 1f);
			delay = swingDelay;
		}

		if(delay > 0f)
		{
			delay -= Time.deltaTime;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag ("Enemy")) //enemy hit
		{				
			other.gameObject.SendMessage ("applyEnemyDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
		}

		if(other.gameObject.CompareTag ("Breakable"))
		{
			other.gameObject.SendMessage ("applyObjectDamage", damage, SendMessageOptions.DontRequireReceiver); // and consume its health
		}
	}
}
