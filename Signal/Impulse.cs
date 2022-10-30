using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using MHPuzzle.Elements;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using MHPuzzle.Effects;
using MHPuzzle.Signal;

namespace MHPuzzle.Impulse
{
	public class Impulse : MonoBehaviour, IImpulse
	{
		[InjectField]
		private IMotherBoard motherBoard;

		[Header("Blink")]

		[SerializeField]
		private Color _blinkColor;

		[SerializeField]
		private float _blinkDuration;

		[SerializeField]
		private float _blinkSpeed;

		[SerializeField]
		private float _amplitudeMatch = 0.1f;

		[SerializeField]
		private float _frequencyMatch = 0.1f;

		[SerializeField]
		private float _waveBlendMatch = 0.1f;

		[Header("Debug")]

		[SerializeField]
		private bool Verbose = false;

		[SerializeField]
		public ASignalConnection DebugStartConnection;

		private SignalMatcher signalMatcher;

		private void Awake()
		{
			signalMatcher = new SignalMatcher(_amplitudeMatch, _frequencyMatch, _waveBlendMatch);
		}

		private bool SignalMatch(IPuzzleCircuit circuit, ISignalConnection In)
		{
			if (In.Emplacement?.Element == null)
				return false;

			return signalMatcher.IsMatching(circuit.CurrentSignal, In.Signal);
		}

		private List<ISignalConnection> GetConnectionsByStatus(ISignalConnection.Value status, IPuzzleEmplacement emplacement)
		{
			List<ISignalConnection> entries = new List<ISignalConnection>();

			foreach (var connection in emplacement.Connections)
				if (connection.Status == status)
					entries.Add(connection);

			return entries;
		}

		public void Launch(ISignalConnection StartConnection)
		{
			Debug.LogWarning("No Error Checking !");

			motherBoard.Reload();

			StartCoroutine(LaunchInternal(StartConnection));

			IEnumerator LaunchInternal(ISignalConnection StartConnection)
			{
				yield return StartCoroutine(GenerateImpulse(StartConnection));

				if (Verbose) Debug.Log($"Checking every element : {motherBoard.IsEveryElementConnected()}");

				IEnumerator GenerateImpulse(ISignalConnection connection)
				{
					if (Verbose) Debug.Log($"Generating new impulse from {connection}");

					var circuit = connection.Circuit;

					string reason = "";

					if (CanContinue(circuit, ref reason) == false)
					{
						if (Verbose) Debug.Log($"Impulse stopped : {reason}");
						yield break;
					}

					var emplacement = circuit.Entry.Emplacement;
					var element = emplacement.Element;

					motherBoard.Touch(element);

					var connections = GetConnectionsByStatus(ISignalConnection.Value.Exit, emplacement);

					if (Verbose) Debug.Log($"Found {connections.Count} connections for emplacement {emplacement}");

					foreach (var c in connections)
					{
						StartCoroutine(GenerateImpulse(c));
					}

					bool CanContinue(IPuzzleCircuit circuit, ref string reason)
					{
						if (circuit == null)
						{
							reason = "circuit not found";
							return false;
						}
						else if (circuit.Entry?.Emplacement?.Element == null)
						{
							reason = "element not found";
							Blink(circuit.Entry.Emplacement);
							return (false);
						}
						else if (circuit.Entry?.Emplacement?.Element.IsBroken == true)
						{
							reason = "broken element";
							Blink(circuit.Entry.Emplacement.Element);
							return (false);
						}
						else if (SignalMatch(circuit, circuit.Entry) == false)
						{
							reason = "signals don't match";
							Blink(circuit);
							return false;
						}
						return true;
					}
				}
			}
		}

		public void LaunchDefault() => Launch(DebugStartConnection);

		private void Blink<T>(T obj) => StartCoroutine(BoardEffects.Blink(obj, _blinkColor, _blinkDuration, _blinkSpeed));
	}
}