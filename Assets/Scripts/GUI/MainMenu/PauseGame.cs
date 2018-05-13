using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	public static bool isPaused;

	private GameObject mainMenu;
	private GameObject statsMenu;
	private GameObject itemMenu;
	private GameObject mapMenu;
	private GameObject saveMenu;

	void initilizeParameters ()
	{
		isPaused = false;

		mainMenu = transform.GetChild (0).gameObject;
		statsMenu = transform.GetChild (1).gameObject;
		itemMenu = transform.GetChild (2).gameObject;
		mapMenu = transform.GetChild (3).gameObject;
		saveMenu = transform.GetChild (4).gameObject;
	}

	// Use this for initialization
	void Start () {
		initilizeParameters ();
	}

	void pauseGame ()
	{
		Time.timeScale = 0;
		isPaused = true;
		mainMenu.SetActive (true);
	}

	void goBackToMainMenu ()
	{
		mainMenu.SetActive (true);
		statsMenu.SetActive (false);
		itemMenu.SetActive (false);
		mapMenu.SetActive (false);
		saveMenu.SetActive (false);
	}

	void continueGame ()
	{
		if (mainMenu.activeSelf)
		{
			Time.timeScale = 1;
			isPaused = false;
			mainMenu.SetActive (false);
		}
		else
			goBackToMainMenu ();
	}


	void getPauseInputs ()
	{
		if (Input.GetButtonDown ("Pause"))
		{
			if (isPaused)
				continueGame ();
			else
				pauseGame ();
		}
	}

	public void enableStatsMenuFromMainMenu ()
	{
		mainMenu.SetActive (false);
		statsMenu.SetActive (true);
	}

	public void enableItemMenuFromMainMenu ()
	{
		mainMenu.SetActive (false);
		itemMenu.SetActive (true);
	}

	public void enableMapMenuFromMainMenu ()
	{
		mainMenu.SetActive (false);
		mapMenu.SetActive (true);
	}

	public void enableSaveMenuFromMainMenu ()
	{
		mainMenu.SetActive (false);
		saveMenu.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		getPauseInputs ();
	}
}
