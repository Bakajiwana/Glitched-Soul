using UnityEngine;
using System.Collections;

public class PlayerWard : MonoBehaviour 
{
	public float rotateSpeed = 5000f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//rotate around the player
		transform.Rotate (Vector3.up * Time.deltaTime * rotateSpeed);
	}
}
