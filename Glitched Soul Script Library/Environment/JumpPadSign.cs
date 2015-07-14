using UnityEngine;
using System.Collections;

//If player is on jump pad send feedback to show player that the jump pad is jumpable

public class JumpPadSign : MonoBehaviour 
{
	public Transform blue;		//means jump pad is ready to use
	public Transform yellow;	//means jump pad is not ready to use

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If player is on jump pad show blue and ready to jump else jsut yellow.
		if(PlayerJumpPad.inJumpZone)
		{
			blue.gameObject.SetActive (true);
			yellow.gameObject.SetActive (false);
		}
		else
		{
			blue.gameObject.SetActive (false);
			yellow.gameObject.SetActive (true);
		}
	}
}
