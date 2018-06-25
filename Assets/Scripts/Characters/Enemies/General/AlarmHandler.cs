using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmHandler : MonoBehaviour {

	private static List <EnemyAI> enemiesOnAlarm;

	void initializeParameters ()
	{
		enemiesOnAlarm = new List <EnemyAI> ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	public static void addEnemyOnAlarm (EnemyAI enemy)
	{
		if (enemiesOnAlarm.Count == 0)
		{
			BGMHandlerScript.chageToPursuit ();
		}

		enemiesOnAlarm.Add (enemy);
	}

	public static void removeEnemyOnAlarm (EnemyAI enemy)
	{
		if (enemiesOnAlarm.Contains (enemy))
		{
			enemiesOnAlarm.Remove (enemy);

			if (enemiesOnAlarm.Count == 0)
				BGMHandlerScript.changeToNormal ();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
