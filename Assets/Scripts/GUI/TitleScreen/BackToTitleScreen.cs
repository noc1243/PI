using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToTitleScreen : MonoBehaviour {

	private const int titleScreenScene = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void backToTitleSceen ()
	{
		SceneManager.LoadScene (titleScreenScene);
	}
}
