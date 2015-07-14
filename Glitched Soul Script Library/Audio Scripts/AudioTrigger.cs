using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour 
{
	public AudioClip clip;
	private bool oneShot;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player") && !oneShot)
		{
			audio.PlayOneShot (clip);
			oneShot = true;
		}
	}
}
