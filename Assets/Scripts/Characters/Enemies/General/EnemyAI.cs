using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	[SerializeField] private float minimumDistanceToChangeTargets = 0.5f;
	[SerializeField] private float initialSightDistance = 4.0f;
	[SerializeField] private float agentWalkingSpeed = 3.5f;
	[SerializeField] private float agentChasingSpeed = 5.5f;
	[SerializeField] private float chasingMaxTime = 10.0f;
	[SerializeField] private float minHearingDecibelVolume = 50.0f;
	[SerializeField] private LayerMask detectionLayer;

	[Header ("Text GUI Variables")]
	[SerializeField] int numberOfTextShowFrames = 30;
	[SerializeField] int textSize = 50;
	[SerializeField] float distanceToTargetStandard = 17.0f;
	[SerializeField] float textYCorrection = -20.0f;
	[SerializeField] float textXCorrection = -20.0f;
	[SerializeField] float textAscendingSpeed = 5.0f;
	[SerializeField] Color textColor;

	private GUIStyle textStyle;
	private string suspectedString;
	private string detectedString;
	private string showString;
	private int textShowTime;
	private bool hasDetectedOrSuspected;
	private bool onSuspection;
	private bool onDetection;
	private bool showingText;
	private Rect text;

	private Camera mainCamera;

	private int newTarget;
	private bool alreadyReachedTarget;
	private NavMeshAgent agent;
	private Vector3[] standardMovementTargets;
	private Animator enemyAnimator;

	private Vector3 currentTargetPosition;
	private float chasingTime;

	private const string animatorIsLookingAround = "isLookingAround";
	private const string animatorIsSuspecting = "isSuspecting";
	private const string animatorIsDetecting = "isDetecting";

	private const float maxDeviatingDistanceFromPlayer = 0.5f;
	private const float airBetaAttenuationCoefficient = 1.38f;

	private bool verifyIfOnScreen ()
	{
		Vector3 screenPosition = mainCamera.WorldToScreenPoint (transform.position);

		if (screenPosition.x > Screen.width || screenPosition.x < 0.0f || screenPosition.y > Screen.height || screenPosition.y < 0.0f)
			return false;
		else
			return true;
	}

	void OnHearSoundEvent (float soundIntensity )
	{
		if (verifyIfOnScreen ())
		{
			float distance = Vector3.Distance (transform.position, IsMovable.getPlayerPosition ());
			float hearingVolumeIntensity = soundIntensity * Mathf.Exp (-airBetaAttenuationCoefficient * distance);
			float decibelHearingVolumeIntensity = 10 * Mathf.Log10 (hearingVolumeIntensity);

			if (decibelHearingVolumeIntensity > minHearingDecibelVolume)
				suspectPlayer ();
		}
	}

	void setSoundEvents ()
	{
		SoundGameEvent.OnHearSoundEvent += OnHearSoundEvent;
	}

	void Awake ()
	{
		setSoundEvents ();
	}

	void setNewDestinationTarget (Vector3 destination)
	{
		agent.SetDestination (destination);
		currentTargetPosition = destination;
	}

	void setNewDestinationOnNormalPath ()
	{
		setNewDestinationTarget (standardMovementTargets [newTarget]);

		newTarget++;

		if (newTarget == standardMovementTargets.Length)
			newTarget = 0;

		alreadyReachedTarget = false;
	}

	void initializeParameters ()
	{
		newTarget = 0;
		alreadyReachedTarget = false;

		chasingTime = 0.0f;

		standardMovementTargets = new Vector3[transform.parent.childCount - 1];
		
		for (int index = 0; index < transform.parent.childCount - 1; index++)
		{
			standardMovementTargets [index] = transform.parent.GetChild	(index).position;
		}

		agent = GetComponent <NavMeshAgent> ();
		enemyAnimator = GetComponentInParent <Animator> ();

		setNewDestinationOnNormalPath ();
	}

	void initializeGUIParameters ()
	{
		text = new Rect ();

		textStyle = new GUIStyle ();
		textStyle.alignment = TextAnchor.UpperLeft;
		textStyle.fontSize = Screen.height / 2 * 100;
		textStyle.normal.textColor = textColor;

		suspectedString = "??";
		detectedString = "!!";
		showString = "";
		textShowTime = 0;

		hasDetectedOrSuspected = false;
		showingText = false;

		mainCamera = Camera.main;
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
		initializeGUIParameters ();
	}

	bool verifyIfOnCurrentTarget ()
	{
		if (!agent.hasPath)
			return true;
		else
			return false;
	}


	void lookToMovementDirection ()
	{
		transform.LookAt (agent.nextPosition);
	}

	public void suspectPlayer ()
	{
		if (!onSuspection && !onDetection)
		{
			enemyAnimator.SetBool (animatorIsSuspecting, true);
			agent.isStopped = true;
			showString = suspectedString;
			hasDetectedOrSuspected = true;
			onSuspection = true;
			goToPlayerLocation ();
			alreadyReachedTarget = false;
			print ("Suspect!!! ");	
		}
	}

	public void detectPlayer ()
	{
		if (!onDetection)
		{
			enemyAnimator.SetBool (animatorIsDetecting, true);
			agent.isStopped = true;
			showString = detectedString;
			hasDetectedOrSuspected = true;
			onDetection = true;
			agent.speed = agentChasingSpeed;
			alreadyReachedTarget = false;
			print ("Detected!!! ");	
		}

		chasingTime = 0.0f;
	}

	void lookForPlayer ()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, initialSightDistance, detectionLayer))
		{
			if (hit.transform.gameObject.tag == "DetectionTag")
			{
				Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
				detectPlayer ();
			}
		}
	}

	void startWalking ()
	{
		agent.isStopped = false;
	}

	void stopLookingAround ()
	{
		onSuspection = false;
		enemyAnimator.SetBool (animatorIsLookingAround, false);
	}

	void stopSuspecting ()
	{
		enemyAnimator.SetBool (animatorIsSuspecting, false);
	}

	void stopDetecting ()
	{
		enemyAnimator.SetBool (animatorIsDetecting, false);
	}

	void goToPlayerLocation ()
	{
		setNewDestinationTarget (IsMovable.getPlayerPosition ());
	}

	void chasePlayer ()
	{
		if (Vector3.Distance (currentTargetPosition, IsMovable.getPlayerPosition ()) > maxDeviatingDistanceFromPlayer)
			setNewDestinationTarget (IsMovable.getPlayerPosition ());
	}

	void handleEnemyMovement ()
	{
		if (!EnemyIsMovable.getIsPlayerDetected ())
		{
			if (verifyIfOnCurrentTarget () && !alreadyReachedTarget && !onDetection)
			{
				enemyAnimator.SetBool (animatorIsLookingAround, true);
				alreadyReachedTarget = true;
			}
			if (onDetection)
				chasePlayer ();
		}

	}

	private void verifyIfShowingText ()
	{
		if (showingText)
		{
			textShowTime++;

			if (textShowTime == numberOfTextShowFrames)
			{
				textShowTime = 0;
				showingText = false;
				//Debug.Break ();
				//Debug.ClearDeveloperConsole ();
			}
		}
	}

	private void clearDetection ()
	{
		
		if (onDetection)
		{
			chasingTime += Time.deltaTime;

			if (chasingTime >= chasingMaxTime)
			{
				chasingTime = 0.0f;
				onDetection = false;
				agent.speed = agentWalkingSpeed;
				hasDetectedOrSuspected = false;
			}
		}

	}

	// Update is called once per frame
	void Update () {
		handleEnemyMovement ();
		lookToMovementDirection ();
		verifyIfShowingText ();
		clearDetection ();
		lookForPlayer ();
	}

	private void calculateTextPosition ()
	{
		Vector3 textPosition = mainCamera.WorldToScreenPoint (transform.position);

		if (hasDetectedOrSuspected)
		{
			text.y = Screen.height - textPosition.y;
			text.y += textYCorrection / textPosition.z * distanceToTargetStandard;
		}
		else
		{
			text.y = Screen.height - textPosition.y;
			text.y += textYCorrection / textPosition.z * distanceToTargetStandard;
			text.y -= textAscendingSpeed/(textShowTime + 1.0f);
		}


		text.x = textPosition.x;
		text.x += textXCorrection / textPosition.z * distanceToTargetStandard;


		text.width = Screen.width;
		text.height = textSize / textPosition.z * distanceToTargetStandard;
		textStyle.fontSize = (int) (textSize / textPosition.z * distanceToTargetStandard);
	}

	private void drawText ()
	{
		GUI.Label (text, showString, textStyle);
	}

	void OnGUI ()
	{
		if (hasDetectedOrSuspected || showingText)
		{
			calculateTextPosition ();
			drawText ();
			hasDetectedOrSuspected = false;
			showingText = true;
		}
	}


}
