using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable ()
	{
		Time.timeScale = 0.0f;
	}

	void OnDisable ()
	{
		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
