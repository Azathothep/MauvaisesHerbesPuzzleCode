using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Elements
{
	public class CircuitHoverable : MonoBehaviour, IHoverable
	{
		public bool IsSelectable { get => true; }

		[SerializeField]
		private Color hoverColor;

		private Color originalColor;

		private new LineRenderer renderer;

		private void Awake()
		{
			renderer = GetComponentInParent<LineRenderer>();

			originalColor = renderer.material.color;
		}

		public void Hover()
		{
			renderer.material.color = hoverColor;
		}

		public void UnHover()
		{
			renderer.material.color = originalColor;
		}
	}
}
