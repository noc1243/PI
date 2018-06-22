using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightRangeModifier : MonoBehaviour {

	[SerializeField] private float lightRange;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemyDetectionAI [] detectors = other.GetComponentsInChildren <EnemyDetectionAI> ();

			for (int index = 0; index < detectors.GetLength (0); index++)
				detectors [index].getOnLight (lightRange);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemyDetectionAI [] detectors = other.GetComponentsInChildren <EnemyDetectionAI> ();

			for (int index = 0; index < detectors.GetLength (0); index++)
				detectors [index].getOffLight ();
		}
	}
}
