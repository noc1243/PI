using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundGUIScript : MonoBehaviour {

	[SerializeField] private AudioClip knockSound;
	[SerializeField] private Color waveColor;
	[SerializeField] private Color backgroundColor;

	private RawImage image;
	private Texture2D texture;

	private Color [] pixels;

	private const int textureSize = 128;

	void initializeParameters ()
	{
		image = GetComponent <RawImage> ();
		texture = new Texture2D (textureSize, textureSize);
		image.texture = texture;

		pixels = texture.GetPixels ();

		ClearPixels ();

		texture.SetPixels (pixels);
		texture.Apply ();
	}

	void ClearPixels ()
	{
		for (int index = 0; index < pixels.Length; index++)
		{
			pixels [index] = backgroundColor;
		}
	}

	void shiftPixelsOneLine ()
	{
		pixels = texture.GetPixels ();

		for (int index = 0; index < pixels.Length / textureSize; index++)
		{
			for (int index2 = 0; index2 < pixels.Length / textureSize; index2++)
			{
				if (index2 != pixels.Length / textureSize - 1)
				{
					pixels [index * textureSize + index2] = pixels [index * textureSize + index2 + 1];
				}
				else
					pixels [index2] = backgroundColor;
			}
		}

		texture.SetPixels (pixels);

		texture.Apply ();
	}

	void OnHearSoundEvent (float soundIntensity )
	{
		int size = knockSound.samples * knockSound.channels;
		float[] samples = new float[size];
		knockSound.GetData (samples, 0);

		ClearPixels ();

		texture.SetPixels (pixels);

		for (int index = 0; index < size; index++)
		{
			texture.SetPixel (texture.width * index / size, (int) (texture.height * (samples [index] + 1f) / 2), waveColor);
		}

		texture.Apply ();
	}

	void setSoundEvents ()
	{
		SoundGameEvent.OnHearSoundEvent += OnHearSoundEvent;
	}

	void unsetSoundEvents ()
	{
		SoundGameEvent.OnHearSoundEvent -= OnHearSoundEvent;
	}

	void Awake ()
	{
		setSoundEvents ();
	}

	void OnDisable ()
	{
		unsetSoundEvents ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}
		
	// Update is called once per frame
	void Update () {
		shiftPixelsOneLine ();
	}
}
