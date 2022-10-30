using MHPuzzle.Elements;
using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public abstract class AOscilloscope : MonoBehaviour, IOscilloscope
	{
		public ISignal CurrentSignal { get; protected set; }
		public ISignal TargetSignal { get; protected set; }

		[InjectField]
		private Configurator configurator;

		[SerializeField]
		[Range(0.0f, 1.0f)] public float AmplitudeMin;

		[SerializeField]
		[Range(0.0f, 1.0f)] public float AmplitudeMax;

		[SerializeField]
		[Range(0.0f, 1.0f)] public float FrequencyMin;

		[SerializeField]
		[Range(0.0f, 2.0f)] public float FrequencyMax;

		// number of points drawn (the higher, the smoother the wave will be)
		[SerializeField]
		private int _points;

		// the frame dimensions in which the wave will be drawn
		[SerializeField]
		private Vector2 _xLimits = new Vector2(0, 1);

		[SerializeField]
		[Range(0.0f, 3.0f)] private float _movementSpeed = 1.0f;

		[SerializeField]
		private Gradient _matchColor;

		private SignalMatcher signalMatcher;

		[SerializeField]
		private bool InductorAsOffset = false;

		protected void Awake()
		{
			configurator.ConfigureInContext(new _C_WaveDatas(_points, _xLimits, _movementSpeed), this);

			signalMatcher = new SignalMatcher();
		}

		public void DisplayWaves(SignalProperties current, SignalProperties target)
		{
			var currentDisplay = Convertor(current);
			var targetDisplay = Convertor(target);

			if (InductorAsOffset)
				currentDisplay.frequency = targetDisplay.frequency;

			CurrentSignal.SetSignal(currentDisplay);
			TargetSignal.SetSignal(targetDisplay);

			if (signalMatcher.IsMatching(current, target))
				CurrentSignal.SetColor(_matchColor);
			else
				CurrentSignal.ResetColor();
		}

		private SignalDisplay Convertor(SignalProperties signal)
		{
			var display = new SignalDisplay();

			display.amplitude = Mathf.Lerp(AmplitudeMin, AmplitudeMax, signal.amplitude / 100);
			display.frequency = Mathf.Lerp(FrequencyMin, FrequencyMax, signal.frequency / 100);
			display.signalBlend = signal.signalBlend;

			display.offset = 0;
			if (InductorAsOffset)
				display.offset = Mathf.Lerp(_xLimits.x, _xLimits.y, signal.frequency / 100) * 2;

			return display;
		}
	}
}