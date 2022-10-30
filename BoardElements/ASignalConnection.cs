using MHPuzzle.Signal;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace MHPuzzle.Elements
{
	public abstract class ASignalConnection : MonoBehaviour, ISignalConnection
	{
		[SerializeField]
		private bool Verbose;

		public ISignalConnection.Value Status { get; set; }

		public IPuzzleEmplacement Emplacement { get; set; }

		public IPuzzleCircuit Circuit { get; set; }

		public SignalProperties Signal {
			get
			{
				if (Emplacement?.Element == null)
				{
					return new SignalProperties(0, 0);
				}

				return Emplacement.Element.TargetSignal;
			}
		}

		private void Start()
		{
			if (Verbose)
				Debug.Log($"Connection: Emplacement is {Emplacement}, Circuit is {Circuit} and Status is {Status}");
		}
	}
}