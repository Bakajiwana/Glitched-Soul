using UnityEngine;
using System.Collections;

public class ManualScript : MonoBehaviour 
{
	public Transform currentPage;
	public Transform otherPage;
	public Transform otherPage2;

	void OnMouseEnter()
	{
		currentPage.gameObject.SetActive (true);
		otherPage.gameObject.SetActive (false);
		otherPage2.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
