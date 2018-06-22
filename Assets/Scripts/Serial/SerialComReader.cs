using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialComReader : MonoBehaviour {

	public static bool isInSerial = false;

	// Use this for initialization
	void Start () {
		if (isInSerial)
			SERIAL_ARDUINO_.SerialCom.setup ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isInSerial)
			SERIAL_ARDUINO_.SerialCom.getData ();
	}

	void OnDisable ()
	{
		if (isInSerial)
		{
			SERIAL_ARDUINO_.SerialCom.closePort ();
			print ("fechou");
		}
	}
}
