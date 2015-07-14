using UnityEngine;
using System.Collections;

public class PlayerFlashlight : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		light.enabled = !light.enabled;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			light.enabled = !light.enabled;
		}
	}
}
