using UnityEngine;
using System.Collections;

//SCRIPT OBJECTIVE: This script is to manage the idle states and if an idle state is set to an armed state set player to armed state.
//and script weapon visibility

public class PlayerWeaponStates : MonoBehaviour 
{

	//Animator Variables
	private Animator anim;		// a reference to the animator on the character

	//Idle State variables
	/*
	 * 0 = Unarmed Idle State
	 * 1 = Armed Idle State
	*/

	public float idleSet = 0f;	//The number that dictacts what idle animation should be played
	//Mecanim only accepts floats to drive blend trees, not integers. A Reason why it is a float.

	//Weapon Visibility Variables
	public Transform leftSword;
	public Transform rightSword;
	public Transform weapon;
	public Transform weaponHolster;
	public Transform swordHolster;


	// Use this for initialization
	void Start () 
	{
		// initialising reference variables
		anim = GetComponent<Animator>();	//Initialise animator
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Set the idle set mecanim variable to the idleset variable on this script
		anim.SetFloat ("IdleSet", idleSet);

		//if Idle set is more or equal to 1f than the player is definitely armed.
		if(idleSet >= 1f)
		{
			anim.SetBool("Armed", true);
		}
		else if (idleSet == 0f)
		{
			anim.SetBool("Armed", false);

			hideWeapons ();
		}

		// ------------ WEAPON VISIBILITY DEPENDING ON ANIMATIONS --------------------

		//If the player is climbing a ladder: no weapons
		if(anim.GetBool ("Ladder Climb") == true && idleSet >= 1f)
		{
			holsterWeapons ();
		}
		else if(anim.GetBool ("Wall Climb") == true && idleSet >= 1f || anim.GetBool ("Invert Wall Climb") == true && idleSet >= 1f)
		{
			wallClimbSet ();
		}
		else if(anim.GetBool ("Zipline") == true && idleSet >= 1f)
		{
			showWeapons ();
		}

		//During a melee only sword should be shown and gun should be holstered
		else if (anim.GetInteger ("Melee Combo") > 0 || anim.GetBool ("Block") == true 
		    || anim.GetBool("Melee Uppercut") == true && idleSet >= 1f)
		{
			showMelee ();
		}
		//If anything else....
		else if(anim.GetBool ("Grounded") == true && anim.GetBool ("Armed") == true 
		   && anim.GetInteger ("Melee Combo") == 0f && anim.GetBool ("Block") == false 
		   || anim.GetBool("Melee Uppercut") == false && idleSet >= 1f && anim.GetBool ("Zipline") == false
		   && anim.GetBool ("Wall Climb") == false && anim.GetBool ("Ladder Climb") == false)
		{
			showWeapons ();
		}

	}

	public void Unarm ()
	{
		idleSet = 0f;
	}

	public void Arm(float set)
	{
		idleSet = set;

		showWeapons ();
	}


	//Hide all weapons function
	void hideWeapons()
	{
		//No weapon should be shown because he is unarmed
		rightSword.gameObject.SetActive (false);
		weapon.gameObject.SetActive (false);
		leftSword.gameObject.SetActive (false);
		weaponHolster.gameObject.SetActive (false);
		swordHolster.gameObject.SetActive (false);
	}

	void holsterWeapons()
	{
		//No weapon should be shown because he is unarmed
		rightSword.gameObject.SetActive (false);
		weapon.gameObject.SetActive (false);
		leftSword.gameObject.SetActive (false);
		weaponHolster.gameObject.SetActive (true);
		swordHolster.gameObject.SetActive (true);
	}

	//Show all standard weapon set
	void showWeapons()
	{
		//The rifle and the sword holster should be visible but the rest should not.
		rightSword.gameObject.SetActive (false);
		weapon.gameObject.SetActive (true);
		leftSword.gameObject.SetActive (false);
		weaponHolster.gameObject.SetActive (false);
		swordHolster.gameObject.SetActive (true);
	}

	void showMelee()
	{
		rightSword.gameObject.SetActive (true);
		weapon.gameObject.SetActive (false);
		leftSword.gameObject.SetActive (false);
		weaponHolster.gameObject.SetActive (true);
		swordHolster.gameObject.SetActive (false);
	}

	//The left hand sword and standard weapons should be shown
	void wallClimbSet()
	{
		rightSword.gameObject.SetActive (false);
		weapon.gameObject.SetActive (true);
		leftSword.gameObject.SetActive (true);
		weaponHolster.gameObject.SetActive (false);
		swordHolster.gameObject.SetActive (false);
	}
}
