using UnityEngine;
using System.Collections;

//This script is to prevent the end level 3 event from happening when the player respawns

public class Level3CancelEnd : MonoBehaviour 
{
	public Level3End cancelEnd;

	void OnMouseUp()
	{
		cancelEnd.end = false;
	}
}
