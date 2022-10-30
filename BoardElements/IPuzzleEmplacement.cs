using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MHPuzzle.Elements
{
	public interface IPuzzleEmplacement : ISnapSpot<IPuzzleElement>
	{
		public ElementType Type { get; }

		public IPuzzleElement Element { get; }
		public ISignalConnection[] Connections { get; }

		public UnityEvent OnElementSnap { get; }
	}
}