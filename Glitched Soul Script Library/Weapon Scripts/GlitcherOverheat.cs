using UnityEngine;
using System.Collections;

//Script Objective: This is to fix the overheating problem when the player melee's or anything that would
// disable the gun. When the gun is disabled the heat variable stops decreasing and the enumerator stops working.

//Solution make the heat variable a global variable so it will decrease even when the script is not active

public class GlitcherOverheat : MonoBehaviour 
{

	//Overheat variables 
	public static float overHeatCoolDown = 30f;
	public float overHeatMin = 0f;
	public static bool overHeat = false; 
	private float coolDownRate = 1f; 
	public float coolDownPhase1;
	public float coolDownPhase2;
	public static float heat = 0f;
	public float forceCoolDown = 4f;

	//HUD variables
	public Texture2D heatBar;
	public Material heatMat;

	public float heatX;
	public float heatY;
	public float heatW;
	public float heatH;
	
	private float heatLeft;
	private float heatUp;
	private float heatWidth;
	private float heatHeight;

	public Transform overheatWarning;
	public float overheatWarningPrompt = 1.5f;
	public Transform overheated;
	public Transform mousePrompt1;
	public Transform mousePrompt2;

	//Blinking effects
	private bool overheatBlink = false;
	public float overheatBlinkMaxTimer = 1.5f;
	private float overheatBlinkTimer;
	public float overheatBlinkRefresh = -1.5f;

	private bool mousePrompt = false;
	private float promptTimer;
	public float promptMaxTimer = 0.5f;
	public float promptRefresh = -0.5f;


	void OnGUI()
	{
		//Initiate GUI variable results
		heatLeft = Screen.width/ heatX;
		heatUp = Screen.height/ heatY;
		heatWidth = Screen.width/ heatW;
		heatHeight = Screen.height / heatH;
		
		//heat Bar
		Rect square = new Rect (heatLeft, heatUp, heatWidth, heatHeight);
		Graphics.DrawTexture (square, heatBar, heatMat);
	}

	void Start ()
	{
		heat = 0f;
	}

	// Update is called once per frame
	void Update () 
	{
		// IF THE GUN OVERHEATS
		if(heat >= overHeatCoolDown)
		{
			//Set over heat boolean to true
			overHeat = true;
			
			heat = overHeatCoolDown - 0.1f;
		}
		else if(heat > overHeatMin)
		{
			heat -= coolDownRate * Time.deltaTime;
		}
		
		//else if the gun is at minimum heat
		else if (heat <= overHeatMin)
		{
			//Set the over heat boolean to false
			overHeat = false;
		}
		
		//Increase cool down rate in specific areas.
		if(heat <= overHeatCoolDown / 2f)
		{
			coolDownRate = coolDownPhase1;
		}
		else
		{
			coolDownRate = coolDownPhase2;
		}

		//If the gun is over heated the player can repeatedly tap the shoot button to decrease cooling down
		if(overHeat == true)
		{
			if(Input.GetButtonDown ("Fire1"))
			{
				heat -= forceCoolDown;
			}
		}


		//------------Warning prompts for the HUD---------------
		if (overHeat)
		{
			overheatBlink = false;
			mousePrompt = true;

			overheated.gameObject.SetActive (true);
			overheatWarning.gameObject.SetActive (false);
		}
		else if(heat >= overheatWarningPrompt)
		{
			overheatBlink = true;
			mousePrompt = false;
			overheated.gameObject.SetActive (false);
		}
		else
		{
			overheatBlink = false;
			mousePrompt = false;
			overheated.gameObject.SetActive (false);
			overheatWarning.gameObject.SetActive (false);
		}

		if(mousePrompt)
		{
			promptTimer -= Time.deltaTime;
		}
		else
		{
			mousePrompt1.gameObject.SetActive (false);
			mousePrompt2.gameObject.SetActive (false);
		}

		if(promptTimer >= 0f && mousePrompt)
		{
			mousePrompt1.gameObject.SetActive (true);
			mousePrompt2.gameObject.SetActive (false);
		}
		if(promptTimer < 0f && mousePrompt)
		{
			mousePrompt1.gameObject.SetActive (false);
			mousePrompt2.gameObject.SetActive (true);
		}

		if(promptTimer < promptRefresh && mousePrompt)
		{
			promptTimer = promptMaxTimer;
		}

		if(overheatBlink && !overHeat)
		{
			overheatBlinkTimer -= Time.deltaTime;
		}

		if(overheatBlinkTimer >= 0f && overheatBlink && !overHeat)
		{
			overheatWarning.gameObject.SetActive (true);
		}

		if(overheatBlinkTimer <= 0f && overheatBlink && !overHeat)
		{
			overheatWarning.gameObject.SetActive (false);
		}

		if(overheatBlinkTimer <= overheatBlinkRefresh && overheatBlink && !overHeat)
		{
			overheatBlinkTimer = overheatBlinkMaxTimer;
		}


		//----------HUD HEAT BAR-------------
		//Calculate heat percentage
		float heatPercent = 1f - (heat/ overHeatCoolDown);
		if(heatPercent <= 0f)
		{
			heatPercent= 0.001f;
		}
		//Set the player heat to affect the transparency of the heat bar HUD
		heatMat.SetFloat ("_Cutoff", heatPercent);
	}
}
