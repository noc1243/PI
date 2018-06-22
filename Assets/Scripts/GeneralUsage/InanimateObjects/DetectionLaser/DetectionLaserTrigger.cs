using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionLaserTrigger : MonoBehaviour {

	[SerializeField] private float soundIntensity = 100.0f;
	[SerializeField] private float lightRange = 15.0f;

	private GameObject visualCue;

	void initializeParameters ()
	{
		visualCue = transform.GetChild (0).gameObject;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	void autoTurnOffVisualCue ()
	{
		if (lightRange <= IsMovable.getLightRange () && visualCue.activeSelf)
		{
			visualCue.SetActive (false);
		}

		if (lightRange > IsMovable.getLightRange () && !visualCue.activeSelf)
		{
			visualCue.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update () {
		autoTurnOffVisualCue ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			SoundGameEvent.OnHearSoundMethod (soundIntensity);
		}
	}
}
