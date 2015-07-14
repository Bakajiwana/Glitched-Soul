using UnityEngine;
using System.Collections;

public class EnemyFootsteps : MonoBehaviour {

	private Animator anim;				//A variable reference to the animator of the character
	
	public AudioClip footsteps; 	//Footstep sound
	public float audioStepLengthRun = 0.25f;
	private bool step = true;
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
		if(anim.GetFloat ("Speed") > 0f && step == true && !anim.GetBool ("Melee"))
		{
			run ();
		}
	}
	
	
	
	IEnumerator WaitForFootSteps(float stepsLength)
	{
		step = false; 
		yield return new WaitForSeconds(stepsLength);
		step = true;
	}
	
	void run()
	{
		audio.clip = footsteps;
		audio.Play ();
		StartCoroutine (WaitForFootSteps(audioStepLengthRun));
	}
}
