using UnityEngine;
using System.Collections;

public class CameraZoomTrigger : MonoBehaviour 
{
	public int zoomLevel;

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			GameObject.FindGameObjectWithTag("MainCamera").SendMessage ("zoom", zoomLevel);
		}
	}
}
