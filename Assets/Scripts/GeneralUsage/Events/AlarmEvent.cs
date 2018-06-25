using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmEvent : MonoBehaviour {

	public delegate void alarm ();

	public static event alarm OnAlarmEvent;

	public static void OnAlarmMethod ()
	{
		if (OnAlarmEvent != null)
		{
			OnAlarmEvent ();
		}
	}
}
