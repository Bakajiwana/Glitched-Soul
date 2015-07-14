using UnityEngine;
using System.Collections;

public class deathDestroy : MonoBehaviour 
{
	public float destroyTime;

	public GameObject corpse;

	public Transform particles;

	void Update()
	{
		destroyTime -= Time.deltaTime;

		if(destroyTime <= 0f)
		{
			Instantiate (particles, transform.position, Quaternion.identity);
			Destroy (corpse);
		}
	}
}
