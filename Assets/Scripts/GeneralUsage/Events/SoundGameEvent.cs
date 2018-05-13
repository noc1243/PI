using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGameEvent : MonoBehaviour {

	public delegate void hearSound (float soundIntensity);

	public static event hearSound OnHearSoundEvent;

	public static void OnHearSoundMethod (float soundIntensity)
	{
		if (OnHearSoundEvent != null)
		{
			OnHearSoundEvent (soundIntensity);
		}
	}
}
