using MHPuzzle.Signal;
using System.Collections;
using UnityEngine;
using MHPuzzle.Effects;

namespace MHPuzzle.Elements
{
	public class BaseEmplacement : APuzzleEmplacement, IBlink
	{
		[InjectField]
		private AOscilloscope oscilloscope;

		public Renderer Renderer { get; private set; }

		private new void Awake()
		{
			base.Awake();

			Renderer = GetComponentInChildren<SpriteRenderer>();
		}

		public override void Snap(IPuzzleElement element)
		{
			base.Snap(element);

			oscilloscope.DisplayWaves(Connections[0].Circuit.CurrentSignal, element.TargetSignal);
		}
	}
}