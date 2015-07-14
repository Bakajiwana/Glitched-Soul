using UnityEngine;
using System.Collections;
//Script Objective: drop a pick up when killed, with a chance.

//Refence: http://forum.unity3d.com/threads/57562-Random-drop

public class CyborgDropScript : MonoBehaviour 
{
	//Pick up variable 
	public float dropRate = 0.07f; 
	private int randomItem;
	public Transform[] pickUpDrop;
	public Transform dropPosition;

	void Awake()
	{
		randomItem = Random.Range (0, pickUpDrop.Length);
	}

	public void pickUp()
	{
		//if random number generated is within the drop rate chance
		if(Random.Range (0f,1f) <= dropRate)
		{
			//drop the collectible
			Instantiate (pickUpDrop[randomItem], new Vector3(dropPosition.position.x, dropPosition.position.y, 0), transform.rotation);
		}
	}
}
