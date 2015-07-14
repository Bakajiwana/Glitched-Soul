using UnityEngine;
using System.Collections;

public class GlitchCollectSound : MonoBehaviour 
{

	public AudioClip [] glitchSounds;

	public float volume = 0.5f;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			audio.PlayOneShot (glitchSounds[Random.Range (0, glitchSounds.Length)], volume);
		}
	}
}
