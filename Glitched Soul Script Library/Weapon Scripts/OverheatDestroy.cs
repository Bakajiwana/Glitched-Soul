using UnityEngine;
using System.Collections;

//Script Objective: Destroy self when overheat is finished

public class OverheatDestroy : MonoBehaviour {
	

	// Update is called once per frame
	void Update () 
	{
		if(GlitcherOverheat.overHeat == false)
		{
			Destroy (gameObject);
		}
	}
}
