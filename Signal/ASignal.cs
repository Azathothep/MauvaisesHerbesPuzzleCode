using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	[RequireComponent(typeof(LineRenderer))]
	public abstract class ASignal : MonoBehaviour, ISignal
	{
		public SignalDisplay Properties { get => _properties; }

		public Vector3[] Points { get => _points; }

		public void SetSignal(SignalDisplay newProperties) => _properties = newProperties;

		// private

		[SerializeField]
		private SignalType _type = SignalType.Sine;

		protected Vector3[] _points;

		protected LineRenderer m_LineRenderer;

		[Header("Properties")]

		[SerializeField]
		private Gradient _color;

		private Gradient _currentColor;

		[SerializeField]
		private SignalDisplay _properties = new SignalDisplay(0, 0, 0);

		protected int _pointNumber;

		protected Vector2 _xLimits;

		protected float _movementSpeed = 1.0f;

		private float _Tau = 2 * Mathf.PI;

		// Methods

		void Awake()
		{
			m_LineRenderer = GetComponent<LineRenderer>();

			_points = new Vector3[_pointNumber];
			m_LineRenderer.GetPositions(_points);

			_currentColor = _color;
		}

		void Update()
		{
			Draw();
		}

		public void ResetColor() => _currentColor = _color;

		public void SetColor(Gradient newColor) => _currentColor = newColor;

		private void Draw()
		{
			float xStart = _xLimits.x;
			float xFinish = _xLimits.y;

			float speed = Mathf.Pow(10 * _properties.frequency, _movementSpeed);

			m_LineRenderer.positionCount = _pointNumber;
			for (int currentPoint = 0; currentPoint < _pointNumber; currentPoint++)
			{
				float progress = (float)currentPoint / (_pointNumber - 1);
				float x = Mathf.Lerp(xStart, xFinish, progress);

				float y = Y(_type, x, _properties.frequency, Time.timeSinceLevelLoad * speed + _properties.offset);
				foreach (SignalBlend signal in _properties.signalBlend)
				{
					if (signal == null)
						continue;

					float y2 = Y(signal.type, x, _properties.frequency, Time.timeSinceLevelLoad * speed);
					y = Mathf.Lerp(y, y2, signal.blend);
				}

				y *= _properties.amplitude;
				m_LineRenderer.SetPosition(currentPoint, new Vector3(x, y, transform.position.z));
			}

			m_LineRenderer.colorGradient = _currentColor;
		}

		private float Y(SignalType type, float t, float f, float phi)
		{
			if (type == SignalType.Sine)
				return Sine(t, f, phi);
			else if (type == SignalType.Square)
				return Square(t, f, phi);
			else if (type == SignalType.Triangle)
				return Triangle(t, f, phi);
			else if (type == SignalType.Sawtooth)
				return Sawtooth(t, f, phi);
			return Sine(t, f, phi);

			float Sine(float t, float f, float phi) => Mathf.Sin(_Tau * f * t + phi);

			float Square(float t, float f, float phi) => Mathf.Sign(Sine(t, f, phi));

			float Triangle(float t, float f, float phi) => 2 * Mathf.Abs(Sawtooth(t, f, phi)) - 1;

			float Sawtooth(float t, float f, float phi)
			{
				var cot = 1 / Mathf.Tan(Mathf.PI * t * f + (phi / 2));

				return -(2 / Mathf.PI) * Mathf.Atan(cot);
			}
		}
	}
}