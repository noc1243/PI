using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameConverter : MonoBehaviour {

	private static int targetFrameRate;
	private static float deltaTime;
	private static float fps;

	void initializeParameters ()
	{
		targetFrameRate = 60;
		deltaTime = 0.0f;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		fps = 1.0f / deltaTime;
	}

	public static int convertFrameRate (int frameTime)
	{
		if (fps >= 25.0f)
		{
			//print (frameTime);
			int targetTime = (int)(frameTime * fps / targetFrameRate);
			return targetTime;
		}
		else
			return frameTime;
	}
}
