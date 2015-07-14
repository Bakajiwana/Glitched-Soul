using UnityEngine;
using System.Collections;

public class ZiplineScript : MonoBehaviour 
{
	static public bool zipLine = false;

	public Transform startingPos;
	public Transform endingPos;

	// Use this for initialization
	void Start () 
	{

	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			zipLine = true;
		}
	}

	void Update()
	{
		if(zipLine == true)
		{
			//Send start and end point coordinates
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("startPoint", startingPos.position);
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("endPoint", endingPos.position);
			//Find the player and activate the useZipline function
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("useZipline");
		}
	}
}
