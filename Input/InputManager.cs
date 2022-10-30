using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHPuzzle.Elements;

public class InputManager : MonoBehaviour
{
	bool _isDragging = false;

	[SerializeField] IHoverable ToHover;

	[SerializeField] IDraggable ToDrag;

	[SerializeField] IInput input;

	// Offset position between the mouse at the moment of the click and the object to drag position
	Vector2 _mouseOffset;

	private IDraggable GetDraggable(GameObject obj) => obj?.GetComponent<IDraggable>();
	private IHoverable GetHoverable(GameObject obj) => obj?.GetComponent<IHoverable>();
	private IClickable GetClickable(GameObject obj) => obj?.GetComponent<IClickable>();

	public void Constructor(IInput _input)
	{
		input = _input;
	}

    // Update is called once per frame
    void Update()
    {
		UpdateHoverAndDrag();

		void UpdateHoverAndDrag()
		{
			// Free dragged object if releasing the mouse button
			releaseOnMouseUp();

			// Free dragged object if clicking again
			// releaseOnNewClick();

			// If is dragging something, update dragged object position
			// Else, check if a new object is being selected
			if (_isDragging)
			{
				if (ToDrag.IsClickable)
					ToDrag.SetPosition(input.MousePosition - _mouseOffset);
				else
					Release();
			}
			else
			{
				RaycastHit2D hit = Physics2D.Raycast(input.MousePosition, Vector2.zero);

				GameObject HittenObject = hit.collider?.gameObject;

				var Hovering = GetHoverable(HittenObject);

				// If the mouse is hovering something & the element hovered is not the same as the last frame one, show its outline
				// Else, if not hovering anything, hide previously hovered element
				if (Hovering != null && Hovering.IsSelectable)
				{
					if (Hovering != ToHover)
					{
						Hovering.Hover();
						ToHover?.UnHover(); // Hide outline of previously hovered element
						ToHover = Hovering; // Keep track of the element currently being hovered
					}
				}
				else
				{
					ToHover?.UnHover();
					ToHover = null;
				}

				// If the player clicks & the thing is draggable
				if (input.Click)
				{
					var Click = GetClickable(HittenObject);
					if (Click?.IsClickable == true) Click?.OnClick();

					ToDrag = GetDraggable(HittenObject);

					if (ToDrag?.IsClickable == true)
					{
						_mouseOffset = input.MousePosition - ToDrag.GetPosition();
						_isDragging = true;
					}
				}
			}
		}
    }


	private void Release()
	{
		ToDrag?.UnSelect();
		ToDrag = null;
		_isDragging = false;
	}

	private void releaseOnMouseUp()
	{
		if (input.UnClick)
			Release();
	}

	private void releaseOnNewClick()
	{
		if (_isDragging && input.Click)
			Release();
	}
}
