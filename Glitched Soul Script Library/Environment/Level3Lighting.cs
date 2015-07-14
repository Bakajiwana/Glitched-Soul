using UnityEngine;
using System.Collections;

//Script Reference: http://docs.unity3d.com/Documentation/ScriptReference/Light-color.html

//The way this script works is that for each kill the lighting will mover towards the blue light
//This changes the levels overall colour the more enemies are killed

public class Level3Lighting : MonoBehaviour 
{
	
	public Color color0 = Color.red;
	public Color color1 = Color.blue;

	public float maxKill = 100;
	static public float colourChange;

	void Update() 
	{
		colourChange = StatScript.playerKill / maxKill;
		light.color = Color.Lerp(color0, color1, colourChange);
	}
}
