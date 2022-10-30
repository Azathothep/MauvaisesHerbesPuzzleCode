using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHPuzzle.Signal;

namespace MHPuzzle.Elements
{
	public enum ElementType
	{
		Element1,
		Element2,
		Element3,
		Module1,
		Module2,
		Alimentation
	}

	public interface IPuzzleElement : ISnappable<IPuzzleEmplacement>
	{
		public ElementType Type { get; }
		public bool IsBroken { get; }
		public ISignalConnection[] Connections { get; }

		public SignalProperties TargetSignal { get; }
	}
}
