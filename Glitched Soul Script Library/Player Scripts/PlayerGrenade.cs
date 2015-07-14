using UnityEngine;
using System.Collections;

//Script Objective: manage grenade

public class PlayerGrenade : MonoBehaviour 
{
	public int grenadeCount;
	public int grenadeMaxAmount = 3;

	private float grenadeThrowTimer;
	public float grenadeThrowMaxTimer = 0.5f;

	public Transform grenade;
	public float throwForce = 50f;
	private bool throwing = false;

	public Transform grenadeNode;

	private Animator anim;				//A variable reference to the animator of the character

	//3D Text of Grenade Count
	public TextMesh grenadeCountText;

	// Use this for initialization
	void Start () 
	{
		//Initialise player animator
		anim = GetComponent<Animator>();

		grenadeCountText.text = "x" + grenadeCount;		//update grenade count text mesh
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(grenadeCount > grenadeMaxAmount)
		{
			grenadeCount = 3; //prevent having more grenades the player can carry
			grenadeCountText.text = "x" + grenadeCount;		//update grenade count text mesh
		}

		if(Input.GetKeyDown(KeyCode.G) && throwing == false)
		{
			if(grenadeCount > 0)
			{
				anim.SetBool ("Grenade Throw", true);
				grenadeThrowTimer = grenadeThrowMaxTimer;
				throwing = true;
			}
		}

		if(grenadeThrowTimer >= 0f)
		{
			grenadeThrowTimer -= Time.deltaTime;
		}
		else
		{
			if(throwing)
			{
				foreach (Transform child in grenadeNode)
				{
					//throw grenade
					Transform throwGrenade = Instantiate (grenade, child.position, child.rotation) as Transform;
					throwGrenade.rigidbody.AddForce (transform.right * throwForce);
					grenadeCount --;								//Deduct grenade count
					grenadeCountText.text = "x" + grenadeCount;		//update grenade count text mesh
				}
				throwing = false;
			}
			anim.SetBool ("Grenade Throw", false);
		}
	}

	//Function is used to add grenades when player picks up grenades
	public void pickUpGrenade(int _amount)
	{
		grenadeCount += _amount;
		grenadeCountText.text = "x" + grenadeCount;		//update grenade count text mesh
	}
}
