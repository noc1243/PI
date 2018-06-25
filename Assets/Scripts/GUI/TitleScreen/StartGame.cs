using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	private const int gameScene = 1;
	private const string gameMode = "GameMode";


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startGameSensors ()
	{
		PlayerPrefs.SetInt (gameMode, 1);
		SceneManager.LoadScene (gameScene);
	}

	public void startGame ()
	{
		PlayerPrefs.SetInt (gameMode, 0);
		SceneManager.LoadScene (gameScene);
	}
}
