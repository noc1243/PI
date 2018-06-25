using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {


	[SerializeField] float runningSpeed = 5.0f;
	[SerializeField] float walkingSpeed = 1.0f;
	[SerializeField] float sprintingSpeed = 8.0f;
	[SerializeField] float gravity = 20.0f;
	[SerializeField] public float limitFallSpeed = 30.0f;
	[SerializeField] float initialJumpSpeed = 20.0f;
	[SerializeField] float jumpSpeed = 8.0F;
	[SerializeField] float floatingCoeficient = 5.0f;
	[SerializeField] int minimumAirborneFrames = 5;
	[SerializeField] int numberOfDoubleJumpAirborneFrames = 15;
	[SerializeField] int numberOfDashFrames = 10;
	[SerializeField] float dashMultiplier = 2.0f;
	[SerializeField] int numberOfDashCoolDownFrames = 3;
	[SerializeField] float dashMinimumMovementCondition = 1.5f;
	[SerializeField] int numberOfDashInvincibilityFrames = 17; 

	private CharacterController controller;

	private bool isAirborne;
	private bool isJumping; 
	private bool isDoubleJumping;
	private bool isDashing;
	private bool isInDashCoolDown;
	private bool fallingAfterAttacking;
	private MovementReadings movement; 
	private Vector3 moveDirection = Vector3.zero;
	private int airborneFramesTime;
	private int jumpingTime;
	private int dashingTime;
	private int dashCoolDownTime;
	private Transform cameraTransform;


	void getController ()
	{
		controller = GetComponent<CharacterController>();

		controller.detectCollisions = false;
	}

	void getCamera ()
	{
		cameraTransform = Camera.main.transform;
	}

	void initializeMovementParameters ()
	{
		isAirborne = false;
		isJumping = false;
		isDoubleJumping = false;
		isDashing = false;
		isInDashCoolDown = false;
		fallingAfterAttacking = false;
		airborneFramesTime = 0;
		jumpingTime = 0;
		dashingTime = 0;
		dashCoolDownTime = 0;

		movement = new MovementReadings (0.0f, 0.0f);

		//Time.timeScale = 1.0f; If the game starts having problems when reloading the scene, uncomment this line
	}

	// Use this for initialization
	void Start () {
		getController ();

		getCamera ();

		initializeMovementParameters ();
	}

	void inputReader (ref MovementReadings movement)
	{
		float vertical = Input.GetAxis ("Vertical");
		float horizontal = Input.GetAxis ("Horizontal");
		movement.setVertical (vertical);
		movement.setHorizontal (horizontal);
	}

	bool isFalling (float y)
	{
		if (moveDirection.y - gravity * (1 + FrameConverter.convertFrameRate(airborneFramesTime) / floatingCoeficient) * Time.deltaTime < 0)
			return true;
		else
			return false;
	}

	void setGroundedValues ()
	{
		if (controller.isGrounded) 
		{
			airborneFramesTime = 0;
			jumpingTime = 0;
			isAirborne = false;
			isJumping = false;
			isDoubleJumping = false;
			fallingAfterAttacking = false;

			if (!IsMovable.getIsGrounded ())
				IsMovable.changeGrounded ();
		}
		else 
		{
			airborneFramesTime++;

			if (IsMovable.getIsGrounded ())
				IsMovable.changeGrounded ();
		}

		if (Input.GetButtonDown ("Jump") && !controller.isGrounded && !isDoubleJumping && isAirborne) 
		{
			isDoubleJumping = true;
			jumpingTime = 0;
			airborneFramesTime = numberOfDoubleJumpAirborneFrames;
		}

	}

	float getSpeedInput ()
	{
		if (Input.GetButton ("Run"))
			return sprintingSpeed;
		else if (Input.GetButton ("Walk"))
			return walkingSpeed;
		else
			return runningSpeed;
	}

	void setMovingInputs ()
	{
		moveDirection = new Vector3 (movement.getHorizontal (), 0.0f, movement.getVertical ());
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection = Quaternion.AngleAxis (cameraTransform.eulerAngles.y, Vector3.up) * moveDirection;
		moveDirection *= getSpeedInput ();
	}

	bool getJumpingInput ()
	{
		if ((Input.GetButtonDown ("Jump")) || (Input.GetButton ("Jump") && isJumping) || (jumpingTime > 0 && jumpingTime < minimumAirborneFrames && !isDashing && !Input.GetButton ("Fire1")))
			return true;
		else
			return false;
	}

	void setJumpingInputs ()
	{
		if (getJumpingInput ()) {
			if (controller.isGrounded || isDoubleJumping) 
			{
				moveDirection.y = initialJumpSpeed;	
				isJumping = true;
				isAirborne = true;
			}
			else
				moveDirection.y = jumpSpeed;

			jumpingTime++;
		}
	}

	void verifyJumping ()
	{
		if (isJumping && Input.GetButtonUp ("Jump"))
			isJumping = false;

		if (isFalling(moveDirection.y))
			moveDirection.y = 0;
	}

	void fallUnderGravity ()
	{
		moveDirection.y -= gravity * (1 + FrameConverter.convertFrameRate(airborneFramesTime)/floatingCoeficient) * Time.deltaTime;
		if (moveDirection.y < -limitFallSpeed)
			moveDirection.y = -limitFallSpeed;
	}

	void dash ()
	{
		if (!isDashing) 
		{
			jumpingTime = 0;
			isInDashCoolDown = true;
			isDashing = true;
		}

		moveDirection *= dashMultiplier;

		dashingTime++;
		if (dashingTime == numberOfDashFrames) 
		{
			isDashing = false;
			dashingTime = 0;
		}
	}

	void verifyDashCoolDown ()
	{
		if (dashCoolDownTime == numberOfDashCoolDownFrames) 
		{
			isInDashCoolDown = false;
			dashCoolDownTime = 0;
		}

		if (isInDashCoolDown) 
		{	
			dashCoolDownTime++;
		}
	}

	bool isMoving ()
	{
		if (moveDirection.magnitude > dashMinimumMovementCondition)
			return true;
		else
			return false;
	}

	void setDashInput ()
	{
		if (((Input.GetButtonDown ("Fire2") && !isInDashCoolDown) || isDashing) && isMoving ()) {
			dash ();
		}

		verifyDashCoolDown ();
	}

	void stopJumpingIfAttacking ()
	{
		if ((Input.GetButtonDown ("Fire1") && !controller.isGrounded && !PauseGame.isPaused) || fallingAfterAttacking)
		{
			jumpingTime = 0;
			fallingAfterAttacking = true;
		}
	}

	void moveCharacter (MovementReadings movement)
	{
		moveDirection = Vector3.zero;

		stopJumpingIfAttacking ();

		if (IsMovable.getIsAbleToMove () && !IsMovable.getIsStunned ()) 
		{
			setGroundedValues ();

			setMovingInputs ();

			fallUnderGravity ();

			controller.Move (moveDirection * Time.deltaTime);
		}
	}

	void updatePlayerPosition ()
	{
		IsMovable.changePlayerPosition (transform.position);
	}

	// Update is called once per frame
	void Update () {
		inputReader (ref movement);
		moveCharacter (movement);
		updatePlayerPosition ();
	}

}
