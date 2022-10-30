using MHPuzzle.Elements;
using MHPuzzle.Signal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Elements
{
	public class CircuitDisplayer : MonoBehaviour, IClickable
	{
		public bool IsClickable { get => true; }

		private IPuzzleCircuit circuit;

		private EdgeCollider2D m_edgeCollider;

		private void Awake()
		{

		}

		public void OnClick()
		{
			circuit.ApplyFactors();
		}

		public void Configure()
		{
			circuit = GetComponentInParent<IPuzzleCircuit>();

			m_edgeCollider = GetComponent<EdgeCollider2D>();

			GenerateEdgeCollider();

			void GenerateEdgeCollider()
			{
				var colliderPoints = new List<Vector2>();

				foreach (Vector3 pos in circuit.LineLocalPositions)
				{
					colliderPoints.Add(pos);
				}

				m_edgeCollider.SetPoints(colliderPoints);
			}
		}
	}
}
