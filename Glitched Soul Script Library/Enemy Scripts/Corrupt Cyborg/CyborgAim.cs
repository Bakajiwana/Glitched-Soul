using UnityEngine;
using System.Collections;

public class CyborgAim : MonoBehaviour {

	//Position Variables
	public Transform player;
	public Transform self;
	
	//Object Position Variables
	private Vector2 playerPos;
	private Vector2 selfPos;
	
	//Access Mecanim Variables
	private Animator anim;
	
	//Angle variable
	private float angle;
	
	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//Obtain the player position using the camera
		if(player)
		{
			playerPos = Camera.main.WorldToScreenPoint(player.position);
		}

		selfPos = Camera.main.WorldToScreenPoint(self.position);
		
		//Obtain the difference between the mouse and player coordinates
		selfPos.x = selfPos.x - playerPos.x;
		selfPos.y = selfPos.y - playerPos.y; 
		
		//Use the difference of x/y to find the angle using Atan2(x/y). 
		angle = Mathf.Atan2 (selfPos.x, selfPos.y) * Mathf.Rad2Deg; //This will return aiming coordinates

		anim.SetFloat ("AimCoord", angle);	//Send information to animator for aiming animations
	}
}
