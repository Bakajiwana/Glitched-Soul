using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	//SCRIPT OBJECTIVE: Follow the player smoothly and create a dampening effect

	/*SOURCE CODE: (Lightly tweaked, converted to C# and studied)
	 * http://answers.unity3d.com/questions/29183/2d-camera-smooth-follow.html - 2D Camera Smooth Follow Script
	*/

	//Camera Variables
	public float dampTime = 0.3f; //offset from the viewport center to fix damping
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	

	void Start()
	{

	}
	
	void Update() 
	{
		if(target) 
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
	}
}
