using UnityEngine;
using System.Collections;

//Script Objective: destroy particles

public class particleDestroy : MonoBehaviour 
{
	public float destroyTime;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject,destroyTime);
	}
}
