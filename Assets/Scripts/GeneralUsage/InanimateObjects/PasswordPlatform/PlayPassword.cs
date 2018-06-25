using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPassword : MonoBehaviour {

	private AudioSource passwordPlayer;

	void initializeParameters ()
	{
		passwordPlayer = GetComponent <AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			if (!passwordPlayer.isPlaying)
				passwordPlayer.Play ();
		}
	}
}
