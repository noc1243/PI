using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockSoundScript : MonoBehaviour {

	[SerializeField] private float soundIntensity = 50.0f;

	// Use this for initialization
	void Start () {
		
	}

	void makeSound ()
	{
		if (Input.GetButtonDown ("Fire2"))
		{
			SoundGameEvent.OnHearSoundMethod (soundIntensity);
		}
	}

	// Update is called once per frame
	void Update () {
		makeSound ();
	}
}
