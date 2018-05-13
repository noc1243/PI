using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyIsMovable {

	private static bool isPlayerDetected = false;

	public static void changteIsPlayerDetected ()
	{
		isPlayerDetected = !isPlayerDetected;
	}

	public static bool getIsPlayerDetected ()
	{
		return isPlayerDetected;
	}
}
