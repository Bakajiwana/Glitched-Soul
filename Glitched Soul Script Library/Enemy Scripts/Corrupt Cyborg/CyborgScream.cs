using UnityEngine;
using System.Collections;

public class CyborgScream : MonoBehaviour {

	private Animator anim;				//A variable reference to the animator of the character

	public AudioClip scream;

	public float volume;

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(anim.GetBool ("Alert") == true)
		{
			audio.PlayOneShot (scream, volume);
		}
	}
}
