using System.Collections;
using UnityEngine;
using MHPuzzle.Effects;

namespace MHPuzzle.Elements
{
	public class BaseSingleElement : ABaseElement, IBlink
	{
		public Renderer Renderer { get; private set; }

		private new void Awake()
		{
			base.Awake();

			Renderer = GetComponentInChildren<SpriteRenderer>();
		}
	}
}