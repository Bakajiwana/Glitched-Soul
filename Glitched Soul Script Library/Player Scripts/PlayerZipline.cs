using UnityEngine;
using System.Collections;

//Script Objective: Zipline function

public class PlayerZipline : MonoBehaviour 
{
	//Zipline variables
	public float time = 5f;
	private float elapsedTime = 0f;

	private Vector3 startPos;
	private Vector3 endPos;

	private Animator anim;			//A variable reference to the animator of the character


	void Start ()
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}


	//Get the locations of the start and end point from another script (Zipline Script)
	public void startPoint(Vector3 start)
	{
		startPos = start;
	}

	public void endPoint(Vector3 end)
	{
		endPos = end;
	}

	//Then zipline between zipline points
	public void useZipline()
	{
		//While the elapsed time is less than time
		if (elapsedTime < time)
		{
			Vector3 startArea = startPos;
			Vector3 destination = endPos;
			//Move between the two points
			transform.position = Vector3.Lerp (startArea, destination, (elapsedTime / time));
			elapsedTime += Time.deltaTime;

			anim.SetBool ("Zipline", true);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Zipline End Point"))
		{
			ZiplineScript.zipLine = false;
			anim.SetBool ("Zipline", false);
		}
	}
}
