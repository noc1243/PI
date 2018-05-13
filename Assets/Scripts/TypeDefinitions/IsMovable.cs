using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class IsMovable  {

	private static bool isAbleToMove = true;
	private static bool isCameraAbleToMove = true;
	private static bool isGrounded = true;
	private static bool isAbleToRotate = true;
	private static bool isAttacking = false;
	private static bool isInAttackCoolDown = false;
	private static bool isLockedOn = false;
	private static bool isStunned = false;
	private static bool isInvincible = false;
	private static bool isInvisible = false;
	private static bool isCriticalDamage = false;

	private static Vector3 playerPosition;
	private static Vector3 lockedOnEnemyPosition;

	private static int comboState = 0;
	private static int attack = 10;
	private static int magicAttack = 10;
	private static float lightRange = 1.0f;

	private static Transform lockedOnEnemy;

	public static void changeMovement ()
	{
		isAbleToMove = !isAbleToMove;
	}

	public static void changeCameraMovement ()
	{
		isCameraAbleToMove = !isCameraAbleToMove;
	}

	public static void changeGrounded ()
	{
		isGrounded = !isGrounded;
	}

	public static void changeIsAbleToRotate ()
	{
		isAbleToRotate = !isAbleToRotate;
	}

	public static void changeIsAttacking ()
	{
		isAttacking = !isAttacking;
	}

	public static void changeComboState (int comboState)
	{
		IsMovable.comboState = comboState;
	}

	public static void changeIsInAttackCoolDown ()
	{
		isInAttackCoolDown = !isInAttackCoolDown;
	}

	public static void changeIsLockedOn ()
	{
		isLockedOn = !isLockedOn;
	}

	public static void changePlayerPosition (Vector3 playerPosition)
	{
		IsMovable.playerPosition = playerPosition;
	}

	public static void changeLockedOnEnemyPosition (Vector3 lockedOnEnemyPosition)
	{
		IsMovable.lockedOnEnemyPosition = lockedOnEnemyPosition;
	}

	public static void changeLockedOnEnemyTransform (Transform lockedOnEnemy)
	{
		IsMovable.lockedOnEnemy = lockedOnEnemy;
	}

	public static void changeAttack (int attack)
	{
		IsMovable.attack = attack;
	}

	public static void changeMagicAttack (int magicAttack)
	{
		IsMovable.magicAttack = magicAttack;
	}

	public static void changeIsStunned () 
	{
		isStunned = !isStunned;
	}

	public static void changeIsInvincible ()
	{
		isInvincible = !isInvincible;
	}

	public static void changeIsInvisible ()
	{
		isInvisible = !isInvisible;
	}

	public static void changeIsCriticalDamage ()
	{
		isCriticalDamage = !isCriticalDamage;
	}

	public static void changeLightRange (float lightRange)
	{
		IsMovable.lightRange = lightRange;
	}

	public static bool getIsAbleToMove ()
	{
		return isAbleToMove;
	}

	public static bool getIsCameraAbleToMove ()
	{
		return isCameraAbleToMove;
	}

	public static bool getIsGrounded ()
	{
		return isGrounded;	
	}

	public static bool getIsAbleToRotate ()
	{
		return isAbleToRotate;
	}

	public static bool getIsAttacking ()
	{
		return isAttacking;
	}

	public static int getComboState ()
	{
		return comboState;
	}

	public static bool getIsInAttackCoolDown ()
	{
		return isInAttackCoolDown;
	}

	public static bool getIsLockedOn ()
	{
		return isLockedOn;
	}

	public static Vector3 getPlayerPosition ()
	{
		return playerPosition;
	}

	public static Vector3 getLockedOnEnemyPosition ()
	{
		return lockedOnEnemyPosition;
	}

	public static Transform getLockedOnEnemyTransform ()
	{
		return lockedOnEnemy;
	}

	public static int getAttack ()
	{
		return attack;
	}

	public static int getMagicAttack ()
	{
		return magicAttack;
	}

	public static bool getIsStunned ()
	{
		return isStunned;
	}

	public static bool getIsInvincible ()
	{
		return isInvincible;
	}

	public static bool getIsInvisible ()
	{
		return isInvisible;
	}

	public static bool getIsCriticalDamage ()
	{
		return isCriticalDamage;
	}

	public static float getLightRange ()
	{
		return lightRange;
	}

}
