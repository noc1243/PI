using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityIndicatorScript : MonoBehaviour {

	[SerializeField] private float finalImageScale = 2.0f;
	[SerializeField] private float minimumLightRange = 4.0f;
	[SerializeField] private float maximumLightRange = 30.0f;

	// Use this for initialization
	void Start () {
		
	}

	void updateIndicatorSize ()
	{
		Vector3 scale = transform.localScale;

		float yScale = finalImageScale * ((IsMovable.getLightRange () - minimumLightRange) / (maximumLightRange - minimumLightRange));
		scale.y = yScale;

		transform.localScale = scale;
	}

	// Update is called once per frame
	void Update () {
		updateIndicatorSize ();
	}
}
