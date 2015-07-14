using UnityEngine;
using System.Collections;

//This script is in charge of the players audio

public class PlayerAudio : MonoBehaviour 
{
	private Animator anim;				//A variable reference to the animator of the character

	public AudioClip[] footsteps; 	//Footstep sound
	public float audioStepLengthRun = 0.25f;
	public float audioStepLengthRunBackward = 0.40f;
	public float audioStepLengthCrouch = 1f;
	public float footStepVolume = 0.5f;
	private bool step = true;
	//Reference: http://answers.unity3d.com/questions/11486/footstep-sounds-when-walking.html

	public AudioClip jump;

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
		if(anim.GetFloat ("Direction") > 0f && anim.GetBool ("Invert") == false && anim.GetBool ("Grounded") == true && step == true &&
		   anim.GetBool ("Crouch") == false || anim.GetFloat ("Direction") < 0f && anim.GetBool ("Invert") == true && anim.GetBool ("Grounded") == true && step == true
		   && anim.GetBool ("Crouch") == false)
		{
			run ();
		}

		if(anim.GetFloat ("Direction") < 0f && anim.GetBool ("Grounded") == true && anim.GetBool ("Invert") == false && step == true && anim.GetBool ("Crouch") == false 
		   || anim.GetFloat ("Direction") > 0f && anim.GetBool ("Grounded") == true && anim.GetBool ("Invert") == true && step == true && anim.GetBool ("Crouch") == false)
		{
			runBackwards ();
		}

		if(anim.GetBool ("Crouch") == true && step == true && anim.GetBool ("Grounded") && anim.GetFloat ("Direction") < 0f ||
		   anim.GetBool ("Crouch") == true && step == true && anim.GetBool ("Grounded") && anim.GetFloat ("Direction") > 0f)
		{
			crouch ();
		}

		if (anim.GetBool ("Grounded")&& Input.GetKeyDown (KeyCode.Space))
		{
			audio.Stop ();
			audio.clip = jump;
			audio.Play ();
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
		audio.clip = footsteps[Random.Range (0, footsteps.Length)];
		audio.volume = footStepVolume;
		audio.Play ();
		StartCoroutine (WaitForFootSteps(audioStepLengthRun));
	}

	void runBackwards()
	{
		audio.clip = footsteps[Random.Range (0, footsteps.Length)];
		audio.volume = footStepVolume;
		audio.Play ();
		StartCoroutine (WaitForFootSteps(audioStepLengthRunBackward));
	}

	void crouch()
	{
		audio.clip = footsteps[Random.Range (0, footsteps.Length)];
		audio.volume = footStepVolume;
		audio.Play ();
		StartCoroutine (WaitForFootSteps(audioStepLengthCrouch));
	}
}
