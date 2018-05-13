using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePlayerPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		IsMovable.changePlayerPosition (transform.position);
	}
}
