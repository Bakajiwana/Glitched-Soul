using UnityEngine;
using System.Collections;

//Change Z Axis of the camera

public class CameraZoom : MonoBehaviour 
{
	private int zoomlevel;

	public float speed;

	public float zoom1 = -22f;
	public float zoom2 = -27f;
	public float zoom3 = -38f;

	void Update ()
	{
		if(zoomlevel == 1)
		{
			float step = speed * Time.deltaTime;
			Vector3 mainZ = new Vector3 (transform.position.x, transform.position.y, zoom1);
			transform.position = Vector3.MoveTowards(transform.position, mainZ, step);
		}
		if(zoomlevel == 2)
		{
			float step = speed * Time.deltaTime;
			Vector3 mainZ = new Vector3 (transform.position.x, transform.position.y, zoom2);
			transform.position = Vector3.MoveTowards(transform.position, mainZ, step);
		}
		if(zoomlevel == 3)
		{
			float step = speed * Time.deltaTime;
			Vector3 mainZ = new Vector3 (transform.position.x, transform.position.y, zoom3);
			transform.position = Vector3.MoveTowards(transform.position, mainZ, step);
		}
	}

	public void zoom(int _level)
	{
		zoomlevel = _level;
	}
}
