using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInput : IInput
{
	public void ReadInput()
	{
		if (IsEnabled == false)
			return;

		Click = Input.GetMouseButtonDown(0);
		UnClick = Input.GetMouseButtonUp(0);
		MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Scroll = Input.mouseScrollDelta.y;
	}

	public void Enable() => IsEnabled = true;

	public void Disable()
	{
		Click = false;
		IsEnabled = false;
	}

	public bool Click { get; private set; }
	public bool UnClick { get; private set; }
	public Vector2 MousePosition { get; private set; }
	public float Scroll { get; private set; }

	private bool IsEnabled;
}
