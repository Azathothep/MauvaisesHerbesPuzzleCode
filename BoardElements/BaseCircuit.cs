using System.Collections;
using UnityEngine;
using MHPuzzle.Effects;

namespace MHPuzzle.Elements
{
	public class BaseCircuit : APuzzleCircuit, IBlink
	{
		public Renderer Renderer { get; private set; }

		private new void Awake()
		{
			base.Awake();

			Renderer = GetComponentInChildren<LineRenderer>();

			GetComponentInChildren<CircuitDisplayer>()?.Configure();
		}
	}
}