using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMHandlerScript : MonoBehaviour {

	[Header ("BGM Fade Speed")]
	[SerializeField] private float fadeSpeed = 0.05f;

	[Header ("BGMS")]
	[SerializeField] private AudioClip normalBGM;
	[SerializeField] private AudioClip pursuitBGM;

	private static bool isChagingToNormal;
	private float initialVolume;

	private static AudioSource bgmAudioSource;
	private static AudioClip staticNormalBGM;
	private static AudioClip staticPursuitBGM;

	void initializeParameters ()
	{
		isChagingToNormal = false;

		bgmAudioSource = GetComponent <AudioSource> ();
		staticNormalBGM = normalBGM;
		staticPursuitBGM = pursuitBGM;

		initialVolume = bgmAudioSource.volume;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	public static void chageToPursuit ()
	{
		bgmAudioSource.Stop ();
		bgmAudioSource.clip = staticPursuitBGM;
		bgmAudioSource.Play ();
	}

	public static void changeToNormal ()
	{
		isChagingToNormal = true;
	}

	void crossFadeBGMS ()
	{
		if (isChagingToNormal)
		{
			if (bgmAudioSource.clip == pursuitBGM)
			{
				bgmAudioSource.volume -= fadeSpeed;

				if (bgmAudioSource.volume <= 0.0f)
				{
					bgmAudioSource.volume = 0.0f;
					bgmAudioSource.Stop ();
					bgmAudioSource.clip = normalBGM;
					bgmAudioSource.Play ();
				}
			}

			if (bgmAudioSource.clip == normalBGM)
			{
				bgmAudioSource.volume += fadeSpeed;

				if (bgmAudioSource.volume >= initialVolume)
				{
					bgmAudioSource.volume = initialVolume;
					isChagingToNormal = false;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		crossFadeBGMS ();
	}
}
