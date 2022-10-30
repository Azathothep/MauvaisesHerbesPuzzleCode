using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public class SignalMatcher
	{
		private static float _amplitudeOffset;
		private static float _frequencyOffset;
		private static float _waveBlendOffset;

		public SignalMatcher() { }

		public SignalMatcher(float amplitudeOffset, float frequencyOffset, float waveBlendOffset)
		{
			_amplitudeOffset = amplitudeOffset;
			_frequencyOffset = frequencyOffset;
			_waveBlendOffset = waveBlendOffset;
		}

		private bool isBetween(float value, float minValue, float maxValue)
		{
			if (value >= minValue && value <= maxValue) return true;

			return false;
		}

		public bool IsMatching(SignalProperties currentSignal, SignalProperties targetSignal)
		{
			if (isBetween(currentSignal.amplitude, targetSignal.amplitude - _amplitudeOffset, targetSignal.amplitude + _amplitudeOffset) == false)
			{
				Debug.Log($"SignalMatcher: Wrong amplitude (current is {currentSignal.amplitude}, target is {targetSignal.amplitude})");
				return false;
			}

			if (isBetween(currentSignal.frequency, targetSignal.frequency - _frequencyOffset, targetSignal.frequency + _frequencyOffset) == false)
			{
				Debug.Log($"SignalMatcher: Wrong frequency (current is {currentSignal.frequency}, target is {targetSignal.frequency})");
				return false;
			}

			if (currentSignal.signalBlend.Count != targetSignal.signalBlend.Count)
			{
				Debug.Log($"SignalMatcher: Wrong waveblend (current count: {currentSignal.signalBlend.Count}, target count : {targetSignal.signalBlend.Count})");
				return false;
			}

			var currentBlend = new List<SignalBlend>(currentSignal.signalBlend);

			foreach (var targetWave in targetSignal.signalBlend)
			{
				var count = currentBlend.Count;

				foreach (var currentWave in currentBlend)
				{
					if (currentWave.type == targetWave.type && isBetween(currentWave.blend, targetWave.blend - _waveBlendOffset, targetWave.blend + _waveBlendOffset))
					{
						currentBlend.Remove(currentWave);
						break;
					}
				}

				if (currentBlend.Count == count)
				{
					Debug.Log($"SignalMatcher: Wrong waveblend");
					return false;
				}
			}

			return true;
		}
	}
}
