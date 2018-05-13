using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class FogClearScript : MonoBehaviour {

	[SerializeField] Transform miniMapCamera;
	[SerializeField] int textureSize = 256;
	[SerializeField] float revealingRadiusX = 5;
	[SerializeField] float revealingRadiusY = 5;
	[SerializeField] float mapAlphaCorrection = 0.05f;
	[SerializeField] Color fogColor;
	[SerializeField] public int keyID;
	[SerializeField] float lightIncreaseSensitivity = 0.01f;
	[SerializeField] float minimumLight = 1.0f;
	[SerializeField] float maximumLight = 2.0f;
	[SerializeField] float minFogEnd = 7.5f;
	[SerializeField] float maxFogEnd = 15.0f;

	private Texture2D texture;
	private Color[] pixels;
	private int pixelPerUnitX;
	private float precisePixelPerUnitX;
	private int pixelPerUnitY;
	private float precisePixelPerUnitY;
	private Vector2 centerPixel;
	private Color transparent;
	private Vector3 playerPosition;
	private float lightIntensity;
	private bool changedRadius;
	private bool clearTexture;
//	private ChangeMapFloor changeMapFloorEnabler;

	private bool hasMapChanged;

	private const string saveMapString = "Map";

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

	void ClearPixels ()
	{
		for (int index = 0; index < pixels.Length; index++)
		{
			pixels [index] = fogColor;
		}
	}

	void initializeParameters ()
	{
		transparent = new Color (0, 0, 0, 0);
		playerPosition = new Vector3 (0, 0, 0);
//		changeMapFloorEnabler = GetComponentInChildren <ChangeMapFloor> ();
		hasMapChanged = false;

		Renderer renderer = GetComponent <Renderer> ();
		Material fogMaterial = null;

		if (renderer != null)
		{
			fogMaterial = renderer.material;
		}

		if (fogMaterial == null)
		{
			Debug.LogError("Material for Fog Of War not found!");
			return;
		}

		texture = new Texture2D (textureSize, textureSize, TextureFormat.RGBA32, false);
		texture.wrapMode = TextureWrapMode.Clamp;

		pixels = texture.GetPixels ();

		ClearPixels ();

		fogMaterial.mainTexture = texture;

		pixelPerUnitX = Mathf.RoundToInt (textureSize / transform.lossyScale.x);

		if (pixelPerUnitX == 0)
			pixelPerUnitX = 1;

		pixelPerUnitY = Mathf.RoundToInt (textureSize / transform.lossyScale.y);

		if (pixelPerUnitY == 0)
			pixelPerUnitY = 1;

		precisePixelPerUnitX = textureSize / transform.lossyScale.x;
		precisePixelPerUnitY = textureSize / transform.lossyScale.y;

		centerPixel = new Vector2 (textureSize * 0.5f, textureSize * 0.5f);

		lightIntensity = 1.0f;

		changedRadius = true;
		clearTexture = false;
	}

//	void saveMapTexture ()
//	{
//		if (hasMapChanged)
//		{
//			string keySaveMap = saveMapString + "_" + SceneManager.GetActiveScene ().buildIndex.ToString () + "_" + keyID.ToString ();
//
//			byte[] image = texture.EncodeToPNG ();
//
//			Game.addToList (keySaveMap, image);
//		}
//	}
//
//	void loadMapTexture ()
//	{
//		string keySaveMap = saveMapString + "_" + SceneManager.GetActiveScene ().buildIndex.ToString () + "_" + keyID.ToString ();
//
//		if (Game.loadObject (keySaveMap) != null)
//		{
//			byte[] image = (byte[])Game.loadObject (keySaveMap);
//
//			texture.LoadImage (image);
//
//			pixels = texture.GetPixels ();
//		}
//
//	}
//
//	void setSaveEvents ()
//	{
//		SaveGameEvent.OnPlayerLoadGameEvent += loadMapTexture;
//		SaveGameEvent.OnPlayerSaveGameEvent += saveMapTexture;
//		SaveGameEvent.OnPlayerChangeSceneEvent += unsetSaveEvents;
//	}
//
//	void unsetSaveEvents ()
//	{
//		SaveGameEvent.OnPlayerLoadGameEvent -= loadMapTexture;
//		SaveGameEvent.OnPlayerSaveGameEvent -= saveMapTexture;
//		SaveGameEvent.OnPlayerChangeSceneEvent -= unsetSaveEvents;
//	}


	void setLightControl ()
	{
		float oldLightIntensity = lightIntensity;

		if (Input.GetAxisRaw ("LightControl") > 0 && lightIntensity != maximumLight)
		{
			lightIntensity += lightIncreaseSensitivity;

			if (lightIntensity > maximumLight)
				lightIntensity = maximumLight;
		}
		if (Input.GetAxisRaw ("LightControl") < 0 && lightIntensity != minimumLight)
		{
			lightIntensity -= lightIncreaseSensitivity;

			if (lightIntensity < minimumLight)
				lightIntensity = minimumLight;
		}

		if (oldLightIntensity != lightIntensity)
		{
			revealingRadiusX = revealingRadiusX / oldLightIntensity * lightIntensity;
			revealingRadiusY = revealingRadiusY / oldLightIntensity * lightIntensity;

			changedRadius = true;

			if (oldLightIntensity > lightIntensity)
				clearTexture = true;
		}
	}

	void Awake ()
	{
		initializeParameters ();
		initializeGUIParameters ();
//		setSaveEvents ();
	}

	void CreateCircle (int originX, int originY, float radiusX, float radiusY, Color changeColor)
	{
		for (int y = (int) (-radiusY * precisePixelPerUnitY); y <= radiusY * precisePixelPerUnitY; ++y)
		{
			for (int x = (int) (-radiusX * precisePixelPerUnitX); x <= radiusX * precisePixelPerUnitX; ++x)
			{
				float preciseTotalDistance = (Mathf.Pow ((float)x, 2) / Mathf.Pow ((radiusX * precisePixelPerUnitX), 2)) + (Mathf.Pow ((float)y, 2) / Mathf.Pow ((radiusY * precisePixelPerUnitY), 2));
				if (preciseTotalDistance <= 1)
				{
					if (preciseTotalDistance < mapAlphaCorrection)
						preciseTotalDistance = 0.0f;

					Color color = changeColor;
					color.a = preciseTotalDistance - mapAlphaCorrection;

					if (color.a < pixels [Mathf.RoundToInt (((originY) + y) * textureSize + originX + x)].a)
						pixels [Mathf.RoundToInt (((originY) + y) * textureSize + originX + x)] = color;
				}
			}
		}
	}

	void ClearCircle (Color changeColor)
	{
		for (int y = 0; y <= textureSize; ++y)
		{
			for (int x = 0; x <= textureSize; ++x)
			{
				Color color = changeColor;
				color.a = 1.0f;

				pixels [Mathf.RoundToInt (y * textureSize + x)] = color;
			}
		}
	}

	void CustomCreateCircle (int originX, int originY, float radiusX, float radiusY, Color changeColor)
	{
		for (int y = (int) (-radiusY * precisePixelPerUnitY); y <= radiusY * precisePixelPerUnitY; ++y)
		{
			for (int x = (int) (-radiusX * precisePixelPerUnitX); x <= radiusX * precisePixelPerUnitX; ++x)
			{
				float preciseTotalDistance = (Mathf.Pow ((float)x, 2) / Mathf.Pow ((radiusX * precisePixelPerUnitX), 2)) + (Mathf.Pow ((float)y, 2) / Mathf.Pow ((radiusY * precisePixelPerUnitY), 2));
				if (preciseTotalDistance <= 1)
				{
					Color color = changeColor;

					if (color.a < pixels [Mathf.RoundToInt (((originY) + y) * textureSize + originX + x)].a)
						pixels [Mathf.RoundToInt (((originY) + y) * textureSize + originX + x)] = color;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {

	}

	private void clearMap ()
	{
		if (clearTexture)
			ClearPixels ();

		playerPosition = IsMovable.getPlayerPosition ();
		Vector3 translatedPos = IsMovable.getPlayerPosition () - transform.position;


		int pixelPosX = Mathf.RoundToInt (translatedPos.x * precisePixelPerUnitX + centerPixel.x);
		int pixelPosY = Mathf.RoundToInt (translatedPos.z * precisePixelPerUnitY + centerPixel.y);


		CreateCircle (pixelPosX, pixelPosY, revealingRadiusX, revealingRadiusY, transparent);


		texture.SetPixels (pixels);
		texture.Apply (false);

		changedRadius = false;
		clearTexture = false;
	}

	public void customClearMap (float revealingRadiusX, float revealingRadiusY)
	{
		if (transform.position.y > IsMovable.getPlayerPosition ().y && transform.position.y < miniMapCamera.position.y)
		{
			playerPosition = IsMovable.getPlayerPosition ();
			Vector3 translatedPos = IsMovable.getPlayerPosition () - transform.position;

			int pixelPosX = Mathf.RoundToInt (translatedPos.x * precisePixelPerUnitX + centerPixel.x);
			int pixelPosY = Mathf.RoundToInt (translatedPos.z * precisePixelPerUnitY + centerPixel.y);


			hasMapChanged = true;
//			changeMapFloorEnabler.isEnabled = true;
			CustomCreateCircle (pixelPosX, pixelPosY, revealingRadiusX, revealingRadiusY, transparent);


			texture.SetPixels (pixels);
			texture.Apply (false);
		}
	}

	void changeFogDensity ()
	{
		float fogEndDistance = minFogEnd + (lightIntensity - minimumLight) * (maxFogEnd - minFogEnd);

		RenderSettings.fogEndDistance = fogEndDistance;
	}

	// Update is called once per frame
	void Update () {
		
		setLightControl ();

		if (changedRadius)
			changeFogDensity ();
//			clearMap ();

	}

	void OnGUI()
	{
		string text = lightIntensity.ToString ();
		GUI.Label(rect, text, style);
	}
}
