using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionAI : MonoBehaviour {

	[SerializeField] private bool isDetectionRange;
	[SerializeField] private float initialSightDistance = 4.0f;
	[SerializeField] private float finalSightDistance = 30.0f;
	[SerializeField] private float sightIncreaseRate = 0.5f;
	[SerializeField] private LayerMask layerMask;

	private EnemyAI enemyAI;
	private float initialLightRange;
	private float sightDistance;
	private bool onLight;
	private float lightRange;

	void initializeParameters ()
	{
		enemyAI = GetComponentInParent <EnemyAI> ();
		initialLightRange = IsMovable.getLightRange ();
		sightDistance = initialSightDistance;
		onLight = false;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	public void getOnLight (float lightRange)
	{
		onLight = true;
		this.lightRange = lightRange;
	}

	public void getOffLight ()
	{
		onLight = false;
		forceUpdateSightDistance ();
	}

	void forceUpdateSightDistance ()
	{
		sightDistance = ((IsMovable.getLightRange () - initialLightRange) * sightIncreaseRate) + initialSightDistance;

		if (sightDistance > finalSightDistance)
			sightDistance = finalSightDistance;
	}

	void updateSightDistance ()
	{
		if (IsMovable.getLightRange () - initialLightRange > 0)
		{
			sightDistance = ((IsMovable.getLightRange () - initialLightRange) * sightIncreaseRate) + initialSightDistance;

			if (sightDistance > finalSightDistance)
				sightDistance = finalSightDistance;
		}

		if (onLight && sightDistance < lightRange)
		{
			sightDistance = lightRange;
		}
	}

	// Update is called once per frame
	void Update () {
		updateSightDistance ();
	}

	void OnTriggerStay (Collider other) {
		if (other.gameObject.tag == "Player")
		{
			RaycastHit hit;

			Vector3 origin = transform.parent.GetChild (0).position;

			Vector3 direction = other.transform.position - origin;
			direction.Normalize ();

			Debug.DrawRay (origin, direction * sightDistance, Color.white);

			if (Physics.Raycast (origin, direction, out hit, sightDistance, layerMask))
			{
				if (hit.transform.gameObject.tag == "Player")
				{
					Debug.DrawRay (origin, direction * hit.distance, Color.yellow);
					if (isDetectionRange)
						enemyAI.detectPlayer ();
					else
						enemyAI.suspectPlayer ();
				}
			}
			

		}
	}

}
