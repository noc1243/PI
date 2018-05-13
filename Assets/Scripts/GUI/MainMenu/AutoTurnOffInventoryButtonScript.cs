using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurnOffInventoryButtonScript : MonoBehaviour {

	[SerializeField] bool whenToDisable;

	void autoDisable ()
	{
		bool isInCombat;

		if (IsMovable.getLockedOnEnemyPosition () == Vector3.zero)
			isInCombat = false;
		else
			isInCombat = true;

		if (isInCombat != whenToDisable)
			gameObject.SetActive (false);
	}

	// Use this for initialization
	void OnEnable () {
		autoDisable ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
