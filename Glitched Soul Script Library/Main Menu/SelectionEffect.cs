using UnityEngine;
using System.Collections;

public class SelectionEffect : MonoBehaviour 
{

	public Transform effect;

	void OnMouseEnter()
	{
		effect.gameObject.SetActive (true);
	}

	void OnMouseExit()
	{
		effect.gameObject.SetActive (false);
	}
}
