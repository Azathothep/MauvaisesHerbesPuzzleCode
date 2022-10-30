using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Signal
{
	[System.Serializable]
	public enum SignalType
	{
		Sine,
		Square,
		Triangle,
		Sawtooth
	}

	[System.Serializable]
	public class SignalBlend
	{
		public SignalType type;
		[Range(0.0f, 1.0f)] public float blend;

		public SignalBlend(SignalType _type, float _blend)
		{
			type = _type;
			blend = _blend;
		}
	}

	public struct SignalDisplay
	{
		public float amplitude;
		public float frequency;
		public float offset;
		public List<SignalBlend> signalBlend;

		public SignalDisplay(float a, float f, float o)
		{
			amplitude = a;
			frequency = f;
			offset = o;
			signalBlend = new List<SignalBlend>();
		}
	}

	[System.Serializable]
	public struct SignalProperties
	{
		[Range(1.0f, 100.0f)] public float amplitude;
		[Range(1.0f, 100.0f)] public float frequency;
		public List<SignalBlend> signalBlend;

		public SignalProperties(float a, float f)
		{
			amplitude = a;
			frequency = f;
			signalBlend = new List<SignalBlend>();
		}

		public SignalProperties(SignalProperties o)
		{
			amplitude = o.amplitude;
			frequency = o.frequency;
			signalBlend = new List<SignalBlend>(o.signalBlend);
		}
	}

	public interface ISignal
	{
		public void SetSignal(SignalDisplay newProperties);
		public void SetColor(Gradient color);
		public void ResetColor();
	}
}