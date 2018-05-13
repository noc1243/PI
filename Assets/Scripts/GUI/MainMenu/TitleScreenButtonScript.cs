using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenButtonScript : MonoBehaviour {

	private const int titleScreenSceneNumber = 0;

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

		if (!IsMovable.getIsAbleToRotate ())
			IsMovable.changeIsAbleToRotate ();

		if (!IsMovable.getIsCameraAbleToMove ())
			IsMovable.changeCameraMovement ();
	}

	public void continueGame ()
	{
		Time.timeScale = 1;
		PauseGame.isPaused = false;

		enableMovement ();
	}

	public void goToTitleScreen ()
	{
		continueGame ();
		SceneManager.LoadScene (titleScreenSceneNumber);
	}
}
