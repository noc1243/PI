using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnPauseMenu : MonoBehaviour {

	[SerializeField] private GameObject pauseMenu;

	// Use this for initialization
	void Start () {
		
	}

	private void pauseMenuInputHandler ()
	{
		if (Input.GetButtonDown ("Pause"))
			pauseMenu.SetActive (!pauseMenu.activeSelf);
	}

	// Update is called once per frame
	void Update () {
		pauseMenuInputHandler ();
	}
}
