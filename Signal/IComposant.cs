using MHPuzzle.Elements;
using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	[System.Serializable]
	public class SignalFactors
	{
		public float amplitude;
		public float frequency;
		public SignalBlend signalBlend;

		public SignalFactors(float a, float f)
		{
			amplitude = a;
			frequency = f;
			signalBlend = null;
		}
	}

	public interface IComposant : ISnappable< IPuzzleCircuit >
	{
		public SignalFactors SignalFactors { get; }

		public bool IsDirty { get; }
		public void SetClean();
	}
}