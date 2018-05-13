using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementReadings {

	private float vertical;
	private float horizontal;
	 
	public MovementReadings ()
	{
	}

	public MovementReadings (float vertical, float horizontal)
	{
		this.vertical = vertical;
		this.horizontal = horizontal;
	}

	public float getHorizontal ()
	{
		return horizontal;
	}

	public float getVertical ()
	{
		return vertical;
	}

	public void setHorizontal (float horizontal)
	{
		this.horizontal = horizontal;		
	}

	public void setVertical (float vertical)
	{
		this.vertical = vertical;
	}
}

