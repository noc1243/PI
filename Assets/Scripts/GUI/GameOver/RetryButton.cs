using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RetryButton : MonoBehaviour {

	private const int gameScene = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void enableMovement ()
	{
		if (!IsMovable.getIsAbleToMove ())
			IsMovable.changeMovement ();
	}

	public void retry()
	{
		Time.timeScale = 1.0f;
		enableMovement ();
		SceneManager.LoadScene (gameScene);
	}
}
