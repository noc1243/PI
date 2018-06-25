using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyIsMovable {

	private static bool isPlayerDetected = false;

	private static float chasingTime = 0.0f;

	public static void changteIsPlayerDetected ()
	{
		isPlayerDetected = !isPlayerDetected;
	}

	public static bool getIsPlayerDetected ()
	{
		return isPlayerDetected;
	}

}
