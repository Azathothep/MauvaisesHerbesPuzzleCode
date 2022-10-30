using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoverable : IMouseInteractable
{
	public bool IsSelectable { get; }
	public void Hover();
	public void UnHover();
}
