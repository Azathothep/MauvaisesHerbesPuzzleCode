using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
	public bool IsClickable { get; }
	public void OnClick();
}
