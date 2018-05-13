using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
	
	[SerializeField] float lightIncreaseSensitivity = 0.08f;
	[SerializeField] float minimumLight = 1.0f;
	[SerializeField] float maximumLight = 2.0f;
	[SerializeField] float minimumLightRange = 4.0f;
	[SerializeField] float maximumLightRange = 30.0f;

	private float lightIntensity;
	private Light lt;

	void setLightControl ()
	{
		if (Input.GetAxisRaw ("LightControl") > 0 && lightIntensity != maximumLight)
		{
			lightIntensity += lightIncreaseSensitivity;

			if (lightIntensity > maximumLight)
				lightIntensity = maximumLight;

			float lightRange = minimumLightRange + (lightIntensity - minimumLight) * (maximumLightRange - minimumLightRange);
			lt.range = lightRange;

			IsMovable.changeLightRange (lightRange);
		}
		if (Input.GetAxisRaw ("LightControl") < 0 && lightIntensity != minimumLight)
		{
			lightIntensity -= lightIncreaseSensitivity;

			if (lightIntensity < minimumLight)
				lightIntensity = minimumLight;

			float lightRange = minimumLightRange + (lightIntensity - minimumLight) * (maximumLightRange - minimumLightRange);
			lt.range = lightRange;

			IsMovable.changeLightRange (lightRange);
		}
	}

	GUIStyle style;
	Rect rect;
	int w;
	int h;

	void initializeGUIParameters ()
	{
		style = new GUIStyle ();
		w = Screen.width;
		h = Screen.height;

		rect = new Rect(0, 0, w, h * 2 / 100);

		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (255f, 255f, 255f, 1.0f);


		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	void initializeParameters ()
	{
		lt = GetComponent <Light> (); 

		lightIntensity = 1.0f;
	}

	void setInitialParameters ()
	{
		IsMovable.changeLightRange (minimumLightRange);
	}

	void Awake ()
	{
		setInitialParameters ();
	}

	// Use this for initialization
	void Start () {
		initializeGUIParameters ();
		initializeParameters ();
	}
	
	// Update is called once per frame
	void Update () {
		setLightControl ();
	}

	void OnGUI()
	{
		string text = lightIntensity.ToString ();
		GUI.Label(rect, text, style);
	}
}
