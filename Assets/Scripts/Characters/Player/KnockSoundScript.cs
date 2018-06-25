using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockSoundScript : MonoBehaviour {

	[SerializeField] private int threshold = 50;
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
		if (!SerialComReader.isInSerial)
		{
			if (Input.GetButtonDown ("Fire2") && !knockSound.isPlaying)
			{
				SoundGameEvent.OnHearSoundMethod (soundIntensity);
				//knockSound.Play ();
			}
		}
		else
		{
			if (SERIAL_ARDUINO_.SerialCom.previousData [1] > threshold)
			{
				SoundGameEvent.OnHearSoundMethod ((float) SERIAL_ARDUINO_.SerialCom.previousData [1]);
				//knockSound.Play ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		makeSound ();
	}
}
