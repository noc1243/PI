using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	[SerializeField] public bool isLocked = false;
	[SerializeField] bool isMatrix = false;
	[SerializeField] int numberOfColumns = 3;


	[HideInInspector] public int selectedButton;

	private PauseGame buttonClicks;

	private Button[] menuButtons;

	void initializeParameters ()
	{
		selectedButton = 0;

		buttonClicks = GetComponentInParent <PauseGame> ();

		menuButtons = GetComponentsInChildren<Button> ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}

	void highlightButton ()
	{
		if (selectedButton < menuButtons.Length)
		{
			menuButtons [selectedButton].Select ();
			menuButtons [selectedButton].OnSelect (null);
		}
	}

	void changeSelectedButton ()
	{
		if (!isMatrix)
		{
			if (Input.GetButtonDown ("MenuSelection"))
			{
				selectedButton += (int)Input.GetAxisRaw ("MenuSelection");

				if (selectedButton >= menuButtons.Length)
					selectedButton = 0;

				if (selectedButton < 0)
					selectedButton = menuButtons.Length - 1;
			}
		}
		else
		{
			if (Input.GetButtonDown ("MenuSelection"))
			{
				selectedButton += (int)Input.GetAxisRaw ("MenuSelection") * numberOfColumns;

				if (selectedButton >= menuButtons.Length)
					selectedButton = 0;

				if (selectedButton < 0)
					selectedButton = menuButtons.Length - 1;
			}
			else if (Input.GetButtonDown ("MenuSelectionHorizontal"))
			{
				selectedButton += (int)Input.GetAxisRaw ("MenuSelectionHorizontal");

				if (selectedButton >= menuButtons.Length)
					selectedButton = 0;

				if (selectedButton < 0)
					selectedButton = menuButtons.Length - 1;
			}
		}
	}

	void clickButton ()
	{
		if (Input.GetButtonDown ("ButtonClick") && !isLocked)
			menuButtons [selectedButton].onClick.Invoke ();
	}

	// Update is called once per frame
	void Update () {
		if (!isLocked)
			changeSelectedButton ();

		highlightButton ();
	}

	public void enableStatsMenuFromMainMenu ()
	{
		buttonClicks.enableStatsMenuFromMainMenu ();
	}

	public void enableItemMenuFromMainMenu ()
	{
		buttonClicks.enableItemMenuFromMainMenu ();
	}

	public void enableMapMenuFromMainMenu ()
	{
		buttonClicks.enableMapMenuFromMainMenu ();
	}

	public void enableSaveMenuFromMainMenu ()
	{
		buttonClicks.enableSaveMenuFromMainMenu ();
	}


}
