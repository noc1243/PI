using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockSoundScript : MonoBehaviour {

	[SerializeField] private float soundIntensity = 50.0f;

	private AudioSource knockSound;

	void initializeParameters ()
	{
		knockSound = GetComponent <AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	void makeSound ()
	{
		if (Input.GetButtonDown ("Fire2") && !knockSound.isPlaying)
		{
			SoundGameEvent.OnHearSoundMethod (soundIntensity);
			knockSound.Play ();
		}
	}

	// Update is called once per frame
	void Update () {
		makeSound ();
	}
}
