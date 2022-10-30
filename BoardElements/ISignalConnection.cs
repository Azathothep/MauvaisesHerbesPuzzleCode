using MHPuzzle.Signal;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MHPuzzle.Elements
{
	public interface ISignalConnection
	{
		public enum Value
		{
			Entry,
			Exit
		}

		public Value Status { get; }
		public IPuzzleEmplacement Emplacement { get; set; }
		public IPuzzleCircuit Circuit { get; set; }
		public SignalProperties Signal { get; }
	}
}