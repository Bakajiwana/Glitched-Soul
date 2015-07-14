using UnityEngine;
using System.Collections;

//Fade in script for text, ref: http://answers.unity3d.com/questions/531974/attempting-to-fade-3d-text.html

public class TextFade : MonoBehaviour 
{
	private bool fadeIn = false;
	private bool fadeOut= false;
	public float fadeSpeed = 0.05f;
	public float fadeMaxTimer = 5f;
	private float fadeTimer;
	private Color color;

	private bool oneTouch;

	void Awake()
	{
		color = renderer.material.color;  

		fadeTimer = fadeMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{

		renderer.material.color = color;

		if(fadeIn)
		{
			fadeTimer -= Time.deltaTime;

			color.a += fadeSpeed;

			if(fadeTimer <= 0f)
			{
				fadeIn = false;
				fadeOut = true;
			}
		}

		if(fadeOut)
		{
			color.a -= fadeSpeed;

			if(color.a <= 0f)
			{
				Destroy (gameObject);
			}
		}

		if(!oneTouch)
		{
			color.a = 0f;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.gameObject.CompareTag ("Player") && oneTouch == false)
		{
			fadeIn = true;
			oneTouch = true;
		}
	}
}
