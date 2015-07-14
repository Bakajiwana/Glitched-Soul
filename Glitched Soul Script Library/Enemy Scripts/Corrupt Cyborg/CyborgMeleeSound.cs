using UnityEngine;
using System.Collections;

public class CyborgMeleeSound : MonoBehaviour {

	private Animator anim;				//A variable reference to the animator of the character
	
	public AudioClip footsteps; 	//Footstep sound
	public float meleeLength = 0.25f;
	private bool sound = true;
	//Reference: http://answers.unity3d.com/questions/11486/footstep-sounds-when-walking.html
	
	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Play footsteps
		if(anim.GetBool ("Melee") && sound == true)
		{
			run ();
		}
	}
	
	
	
	IEnumerator WaitForMelee(float stepsLength)
	{
		sound = false; 
		yield return new WaitForSeconds(stepsLength);
		sound = true;
	}
	
	void run()
	{
		audio.clip = footsteps;
		audio.Play ();
		StartCoroutine (WaitForMelee(meleeLength));
	}
}
