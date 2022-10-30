using System.Collections;
using UnityEngine;
using MHPuzzle.Signal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MHPuzzle.Elements
{
	public struct _C_PuzzleCircuit
	{
		public float AmplitudeMin;
		public float AmplitudeMax;
		public float FrequencyMin;
		public float FrequencyMax;

		public _C_PuzzleCircuit(float am, float aM, float fm, float fM)
		{
			AmplitudeMin = am;
			AmplitudeMax = aM;
			FrequencyMin = fm;
			FrequencyMax = fM;
		}
	}

	[RequireComponent(typeof(LineRenderer))]
	[RequireComponent(typeof(EdgeCollider2D))]
	public abstract class APuzzleCircuit : MonoBehaviour, IPuzzleCircuit
	{
		public ISignalConnection Exit { get => _exit; }

		public ISignalConnection Entry { get => _entry; }

		public SignalProperties CurrentSignal { get => _currentSignal; }

		[InjectField]
		private AOscilloscope _oscilloscope;

		[SerializeField]
		private ASignalConnection _exit;

		[SerializeField]
		private ASignalConnection _entry;

		[SerializeField]
		private SignalProperties _currentSignal;

		private LineRenderer m_lineRenderer;

		private EdgeCollider2D m_edgeCollider;

		private List<IComposant> composants = new List<IComposant>();

		// Datas

		private SignalProperties _baseSignal { get => Exit.Signal; }

		public Vector3[] LineLocalPositions { get; private set; }
		public Vector3[] LinePositions { get; private set; }
		public float LineLength { get; private set; }
		public float DistanceToUnsnap;

		protected virtual void Awake()
		{
			if (_entry)
			{
				_entry.Circuit = this;
				_entry.Status = ISignalConnection.Value.Entry;
			}
			if (_exit)
			{
				_exit.Circuit = this;
				_exit.Status = ISignalConnection.Value.Exit;
			}

			m_lineRenderer = GetComponent<LineRenderer>();
			m_edgeCollider = GetComponent<EdgeCollider2D>();

			if (_currentSignal.signalBlend == null)
				_currentSignal.signalBlend = new List<SignalBlend>();

			LineLocalPositions = GetLineLocalPositions();

			LinePositions = GetLineWorldPositions();

			GenerateEdgeCollider();

			LineLength = GetLineLength();

			Exit?.Emplacement?.OnElementSnap.AddListener(ApplyFactors);
			Entry?.Emplacement?.OnElementSnap.AddListener(ApplyFactors);

			Vector3[] GetLineLocalPositions()
			{
				var linePositionCount = m_lineRenderer.positionCount;

				var _linePositions = new Vector3[linePositionCount];

				m_lineRenderer.GetPositions(_linePositions);

				return _linePositions;
			}

			Vector3[] GetLineWorldPositions()
			{
				var _positions = new Vector3[LineLocalPositions.Length];

				for (int i = 0; i < LineLocalPositions.Length; i++)
				{
					_positions[i] = LineLocalPositions[i] + transform.position;
				}

				return _positions;
			}

			void GenerateEdgeCollider()
			{
				var colliderPoints = new List<Vector2>();

				foreach (Vector3 pos in LineLocalPositions)
				{
					colliderPoints.Add(pos);
				}

				m_edgeCollider.SetPoints(colliderPoints);
			}

			float GetLineLength()
			{
				float length = 0.0f;

				for (int i = 0; i < m_lineRenderer.positionCount - 1; i++)
				{
					length += Vector2.Distance(m_lineRenderer.GetPosition(i), m_lineRenderer.GetPosition(i + 1));
				}

				return length;
			}
		}

		protected void Update()
		{
			//Is one composant dirty?
			foreach (var c in composants)
			{
				if (c.IsDirty)
				{
					ApplyFactors();
					break;
				}
			}
		}

		public void ApplyFactors()
		{
			ResetProperties();

			var finalFactors = new SignalFactors(1, 1);

			bool FrequencyHasBeenModified = false;

			foreach (IComposant c in composants)
			{
				ComputeFactors(c.SignalFactors);
				c.SetClean();
			}

			ComputeProperties(finalFactors);

			_oscilloscope.DisplayWaves(_currentSignal, _entry.Emplacement.Element != null ? _entry.Emplacement.Element.TargetSignal : new SignalProperties(0, 0));

			void ComputeProperties(SignalFactors final)
			{
				_currentSignal.amplitude = MedianLerp(0.0f, _currentSignal.amplitude, 100.0f, final.amplitude);

				if (FrequencyHasBeenModified)
					_currentSignal.frequency = Mathf.Lerp(0.0f, 100.0f, final.frequency);

				float MedianLerp(float min, float median, float max, float t)
				{
					if (t < 1)
						return Mathf.Lerp(min, median, t);
					return Mathf.Lerp(median, max, t - 1);
				}
			}

			void ComputeFactors(SignalFactors _factors)
			{
				finalFactors.amplitude *= _factors.amplitude;

				if (_factors.frequency >= 0.0f)
				{
					FrequencyHasBeenModified = true;
					finalFactors.frequency *= _factors.frequency;
				}

				if (_factors.signalBlend != null)
					_currentSignal.signalBlend.Add(_factors.signalBlend);
			}
		}

		private void ResetProperties()
		{
			_currentSignal.amplitude = _baseSignal.amplitude;
			_currentSignal.frequency = _baseSignal.frequency;
			_currentSignal.signalBlend = new List<SignalBlend>(_baseSignal.signalBlend);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			var composant = collision.gameObject?.GetComponent<IComposant>();

			if (composant != null)
				Snap(composant);
		}

		public void Snap(IComposant composant)
		{
			composant?.Snap(this);
			composants.Add(composant);
		}

		public void UnSnap(IComposant composant)
		{
			composants.Remove(composant);
			ApplyFactors();
		}

		public Vector2 GetPosition() => transform.position;
	}
}