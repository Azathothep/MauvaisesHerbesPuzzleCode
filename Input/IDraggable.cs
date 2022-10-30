using System.Collections;
using UnityEngine;

public interface IDraggable : IClickable
{
	public bool IsSelected { get; }
	public void UnSelect();
	public Vector2 GetPosition();
	public void SetPosition(Vector2 position);
}