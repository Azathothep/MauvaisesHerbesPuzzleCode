using MHPuzzle.Signal;
using System.Collections;
using UnityEngine;
using MHPuzzle.Effects;

namespace MHPuzzle.Elements
{
	public interface IPuzzleCircuit : ISnapSpot< IComposant >
	{
		public ISignalConnection Entry { get; }
		public ISignalConnection Exit { get; }

		public SignalProperties CurrentSignal { get; }

		public void ApplyFactors();

		public Vector3[] LineLocalPositions { get; }
		public Vector3[] LinePositions { get; }
		public float LineLength { get; }
	}
}