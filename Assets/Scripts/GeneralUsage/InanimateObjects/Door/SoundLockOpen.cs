using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundLockOpen : MonoBehaviour {

	[Header ("Sound Properties")]
	[SerializeField] private float minHearingDecibelVolume = 50.0f;
	[SerializeField] private float leaverage = 0.3f;
	[SerializeField] private float [] soundsIntervals;
	[SerializeField] private float minimumTimeDifference = 0.5f;


	private float[] soundBuffers;
	private float[] soundBufferedIntervals;
	private float previousDetectedTime;
	private Camera mainCamera;
	private Animator doorAnimator;
	private bool isOpen;

	private const float airBetaAttenuationCoefficient = 1.38f;

	private const string animatorIsOpenString = "isOpen";

	void initializeParameters ()
	{
		mainCamera = Camera.main;
		isOpen = false;
		previousDetectedTime = 0.0f;
		doorAnimator = GetComponent <Animator> ();

		soundBuffers = new float[soundsIntervals.GetLength (0) + 1];
		soundBufferedIntervals = new float[soundsIntervals.GetLength (0) + 1];

		for (int index = 0; index < soundBuffers.GetLength (0); index++)
		{
			soundBuffers [index] = Time.time;
			soundBufferedIntervals [index] = 0.0f;
		}
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	private bool verifyIfOnScreen ()
	{
		Vector3 screenPosition = mainCamera.WorldToScreenPoint (transform.position);

		if (screenPosition.x > Screen.width || screenPosition.x < 0.0f || screenPosition.y > Screen.height || screenPosition.y < 0.0f)
			return false;
		else
			return true;
	}

	private void addToBuffer (float time)
	{
		for (int index = 0; index < soundBuffers.GetLength (0); index++)
		{
			if (index + 1 < soundBuffers.GetLength (0))
			{
				soundBuffers [index] = soundBuffers [index + 1];
				soundBufferedIntervals [index] = soundBufferedIntervals [index + 1];
			}
			else
			{
				soundBuffers [index] = time;
				soundBufferedIntervals [index] = time - soundBuffers [index - 1];
			}

			//print (index + ": " + soundBufferedIntervals [index]);
		}
	}

	private void openDoor ()
	{
		isOpen = true;
		doorAnimator.SetBool (animatorIsOpenString, isOpen);
	}

	private void checkPassword ()
	{
		int numberOfCorrectInputs = 0;

		for (int index = 1; index < soundBufferedIntervals.GetLength (0); index++)
		{
			print (index + ": " + Mathf.Abs (soundBufferedIntervals [index] - soundsIntervals [index - 1]));

			if (Mathf.Abs (soundBufferedIntervals [index] - soundsIntervals [index - 1]) < leaverage)
				numberOfCorrectInputs++;
		}
			
		print (numberOfCorrectInputs + "_" + (soundBufferedIntervals.GetLength (0) - 1).ToString ());
		if (numberOfCorrectInputs == soundBufferedIntervals.GetLength (0) - 1)
			openDoor ();
	}

	static void ClearConsole () {
		// This simply does "LogEntries.Clear()" the long way:
		var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
		var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
		clearMethod.Invoke(null,null);
	}

	void OnHearSoundEvent (float soundIntensity )
	{
		if (verifyIfOnScreen () && Mathf.Abs (previousDetectedTime - Time.time) > minimumTimeDifference)
		{
			ClearConsole ();
			previousDetectedTime = Time.time;
			float distance = Vector3.Distance (transform.position, IsMovable.getPlayerPosition ());
			float hearingVolumeIntensity = soundIntensity * Mathf.Exp (-airBetaAttenuationCoefficient * distance);
			float decibelHearingVolumeIntensity = 10 * Mathf.Log10 (hearingVolumeIntensity);

			if (decibelHearingVolumeIntensity > minHearingDecibelVolume)
			{
				addToBuffer (Time.time);
				checkPassword ();
			}
		}
	}

	void setSoundEvents ()
	{
		SoundGameEvent.OnHearSoundEvent += OnHearSoundEvent;
	}

	void Awake ()
	{
		setSoundEvents ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
