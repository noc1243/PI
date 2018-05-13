using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetection : MonoBehaviour {

	[SerializeField] private GameObject gameOverScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Enemy")
			gameOverScreen.SetActive (true);
	}
}
