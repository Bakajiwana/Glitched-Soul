using UnityEngine;
using System.Collections;

public class MusicStartLevel03 : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag ("Player"))
		{
			GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("Volume");
			GameObject.FindGameObjectWithTag ("Music Manager").SendMessage ("CombatEngaged");
		}
	}
}
