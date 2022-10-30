using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ImpulseButton : MonoBehaviour, IClickable
{
	public bool IsClickable { get; private set; }

	[Serializable]
	public class ButtonClickedEvent : UnityEvent { }

	[SerializeField]
	private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

	public void Awake()
	{
		IsClickable = true;
	}

	public void OnClick()
	{
		m_OnClick.Invoke();
	}
}
