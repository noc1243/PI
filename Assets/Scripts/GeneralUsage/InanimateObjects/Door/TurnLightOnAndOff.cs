using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLightOnAndOff : MonoBehaviour {

	[SerializeField] private float minRange;
	[SerializeField] private float maxRange;
	[SerializeField] private float timeToFadeIn;
	[SerializeField] private float timeToFadeOff;

	private Light lt;

	private bool fadingDirection;
	private float fadeInPace;
	private float fadeOutPace;

	void initializeParameters ()
	{
		lt = GetComponent <Light> ();
		fadingDirection = false;

		fadeInPace = (maxRange - minRange) / timeToFadeIn;
		fadeOutPace = (maxRange - minRange) / timeToFadeOff;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}
		
	void fadeInAndOutLight ()
	{
		if (!fadingDirection)
		{
			lt.intensity += fadeInPace;

			if (lt.intensity >= maxRange)
			{
				lt.intensity = maxRange;
				fadingDirection = true;
			}
		}
		else
		{
			lt.intensity-= fadeOutPace;

			if (lt.intensity <= minRange)
			{
				lt.intensity = minRange;
				fadingDirection = false;
				gameObject.SetActive (false);
			}
		}
	}

	void OnDisable ()
	{
		lt.intensity = minRange;
		fadingDirection = false;
	}
	
	// Update is called once per frame
	void Update () {
		fadeInAndOutLight ();
	}
}
