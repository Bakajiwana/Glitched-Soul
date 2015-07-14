using UnityEngine;
using System.Collections;

public class CreditScript : MonoBehaviour {

	public float moveSpeed;

	public float creditTimer = 5f;
	

	// Update is called once per frame
	void Update () 
	{
		transform.Translate (Vector3.up * Time.deltaTime * moveSpeed);

		creditTimer -= Time.deltaTime;

		if(creditTimer <= 0f)
		{
			//When loading a new level, make sure the time scale is at 1 to prevent pause state.
			Time.timeScale = 1.0f;
			
			//Go to Main menu
			Application.LoadLevel (0);
		}
	}
}
