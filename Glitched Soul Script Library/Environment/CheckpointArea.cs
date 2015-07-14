using UnityEngine;
using System.Collections;

public class CheckpointArea : MonoBehaviour 
{
	public int respawnNumber;

	private bool checkpointNotify;
	public float notifyMaxTimer = 5f;
	private float notifyTimer;

	public Transform checkpointSign;

	void Start ()
	{
		notifyTimer = notifyMaxTimer;
	}

	void Update ()
	{
		if(checkpointNotify)
		{
			notifyTimer -= Time.deltaTime;
			checkpointSign.gameObject.SetActive (true);
		}

		if(notifyTimer <= 0f)
		{
			checkpointNotify = false;
			notifyTimer = notifyMaxTimer;
			checkpointSign.gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			if(CheckpointScript.respawnPoint < respawnNumber)
			{
				CheckpointScript.respawnPoint = respawnNumber;
				StatScript.playerScore += StatScript.killScore;
				StatScript.killScore = 0;
				PlayerPrefs.SetInt ("Checkpoint Kills", StatScript.playerKill);

				checkpointNotify = true;
			}
		}
	}
}
