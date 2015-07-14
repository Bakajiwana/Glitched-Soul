using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour 
{
	//fade in script for the last finishing end

	public float fadeSpeed = 5f;

	private bool fade;

	private float fastFade = 0f;

	public float endMaxTimer = 5f;
	private float endTimer;

	public Transform gameOver;

	private bool oneShot;
	// Use this for initialization
	void Start () 
	{
		endTimer = endMaxTimer;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (fade)
		{
			fastFade = fastFade + fadeSpeed * Time.deltaTime;
			renderer.material.color = new Color(0f, 255f, 255f, fastFade);

			endTimer -= Time.deltaTime;

			if(endTimer <= 0f && oneShot == false)
			{
				gameOver.gameObject.SetActive (true);
				oneShot = true;
			}
		}
	}

	public void endFade()
	{
		fade = true;
	}
}
