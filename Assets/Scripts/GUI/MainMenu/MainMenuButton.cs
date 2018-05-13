using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler {

	[SerializeField] public int buttonNumber = 0;

	private MainMenu mainMenu;

	void initializeParameters ()
	{
		mainMenu = GetComponentInParent <MainMenu> ();
	}

	// Use this for initialization
	void Start () {
		initializeParameters ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void lockMainMenu ()
	{
		mainMenu.isLocked = true;
	}

	public void unlockMainMenu ()
	{
		mainMenu.isLocked = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mainMenu.selectedButton = buttonNumber;
		if (mainMenu.isLocked)
			unlockMainMenu ();
	}

	public void OnSelect(BaseEventData eventData)
	{
	}
}
