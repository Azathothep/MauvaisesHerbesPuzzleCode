using MHPuzzle.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public abstract class AComposant : IsDraggable, IComposant
	{
		public struct TransformDatas
		{
			public Vector3 pos;
			public Quaternion rot;
			public Transform parent;

			public TransformDatas(Vector3 p, Quaternion r, Transform t)
			{
				pos = p;
				rot = r;
				parent = t;
			}
		}

		public SignalFactors SignalFactors { get => _signalFactors; }

		private SignalFactors _signalFactors;

		[SerializeField]
		private float DistanceToUnsnap = 10.0f;

		protected IPuzzleCircuit circuit;

		public bool IsSnapped { get => circuit != null; }

		private int _firstSegmentPointIndex = -1;

		private TransformDatas originalTransform;

		public bool IsDirty { get; private set; }

		public void SetClean() => IsDirty = false;

		public override void UnSelect()
		{
			base.UnSelect();

			if (IsSnapped == false)
			{
				transform.localPosition = originalTransform.pos;
				transform.localRotation = originalTransform.rot;
			}
		}

		private new void Awake()
		{
			base.Awake();

			_signalFactors = new SignalFactors(1.0f, -1.0f);

			originalTransform = new TransformDatas(	transform.localPosition,
													transform.localRotation,
													transform.parent	);
			circuit = null;

			SetClean();
		}

		public void Snap(IPuzzleCircuit _circuit)
		{
			circuit = _circuit;
		}

		public void UnSnap()
		{
			circuit.UnSnap(this);
			circuit = null;
			_firstSegmentPointIndex = -1;

			transform.SetParent(originalTransform.parent);
		}

		public override void SetPosition(Vector2 position)
		{
			if (IsSnapped == false)
				base.SetPosition(position);
			else
			{
				try
				{
					FollowLine(circuit, position);
				}
				catch
				{
					Debug.Log("Edge touched");
				}

				UnsnapIfTooFar(circuit, position);

				SetFactors(ref _signalFactors);

				IsDirty = true;
			}

			void FollowLine(IPuzzleCircuit circuit, Vector2 position)
			{
				var linePositions = circuit.LinePositions;

				Vector2[] delimitationVectors = GetDelimitationVectors();

				Vector2[] segmentPositions = GetLineSegment();

				Vector2 point = GetPointInSegment();

				transform.eulerAngles = GetRotation(segmentPositions);

				base.SetPosition(point);

				#region debug
				Debug.DrawLine(segmentPositions[0], position, Color.red);
				Debug.DrawLine(segmentPositions[1], position, Color.red);
				#endregion

				/// <summary>
				/// This function returns the vectors marking the delimitations between the different "zone"
				/// the mouse can be in. For the extremities, it takes the orthogonal vector.
				/// </summary>
				Vector2[] GetDelimitationVectors()
				{
					var positionCounts = linePositions.Length;

					var delimitationVectors = new Vector2[positionCounts];

					for (int i = 0; i < positionCounts; i++)
					{
						Vector2 vectorToPreviousPoint = Vector2.zero;
						Vector2 vectorToNextPoint = Vector2.zero;

						// If the point is not the first one
						if (i > 0)
							vectorToPreviousPoint = linePositions[i - 1] - linePositions[i];

						// If the point is not the last one
						if (i < positionCounts - 1)
							vectorToNextPoint = linePositions[i + 1] - linePositions[i];

						// If the point is one of the extremities : get the orthogonal vector and continue for next loop
						if (i <= 0 || i >= positionCounts - 1)
						{
							if (i <= 0)
								delimitationVectors[i] = Vector2.Perpendicular(vectorToNextPoint);
							else
								delimitationVectors[i] = Vector2.Perpendicular(-vectorToPreviousPoint);

							continue;
						}

						// Else, get the angle between the two vectors, then rotate the previousVector halfway to get the delimitation
						var angle = Vector2.SignedAngle(vectorToPreviousPoint, vectorToNextPoint);

						if (angle > 0)
							angle = 360 + angle;

						delimitationVectors[i] = vectorToPreviousPoint.Rotate(angle / 2) * 5.0f;
					}

					return delimitationVectors;
				}

				/// <summary>
				/// Get the points defining the segment in which is the resistance.
				/// </summary>
				Vector2[] GetLineSegment()
				{
					int zoneCount = linePositions.Length - 1;

					int startIndex = 0;

					int maxIndex = 0;

					// If _firstSegmentPointIndex hasn't already been set, then the composant just snapped
					// so we must check every zones to find the right one
					// Else, we must only check the nearby zones (or the composant could jump between faraway segments)
					if (_firstSegmentPointIndex < 0)
					{
						startIndex = 0;
						maxIndex = zoneCount - 1;
					}
					else
					{
						// Get the previous index of the first segment point, unless the first point is 0;
						startIndex = Mathf.Max(0, _firstSegmentPointIndex - 1);
						// Get the next index of the first segment point, unless it' the last point of the line
						maxIndex = Mathf.Min(_firstSegmentPointIndex + 1, zoneCount - 1);
					}

					for (int i = startIndex; i <= maxIndex; i++)
					{
						// The points of the segment currently being tested
						Vector2[] currentSegmentPoints = { linePositions[i], linePositions[i + 1] };

						// The vector from the segment points to the mouse position
						Vector2[] segmentToMouseVectors = { position - currentSegmentPoints[0], position - currentSegmentPoints[1] };

						#region debug
						Debug.DrawLine(currentSegmentPoints[0], currentSegmentPoints[0] + delimitationVectors[i]);
						Debug.DrawLine(currentSegmentPoints[0], currentSegmentPoints[0] - delimitationVectors[i]);
						Debug.DrawLine(currentSegmentPoints[1], currentSegmentPoints[1] + delimitationVectors[i + 1]);
						Debug.DrawLine(currentSegmentPoints[1], currentSegmentPoints[1] - delimitationVectors[i + 1]);
						#endregion

						// Get the angles between each delimitation and its corresponding segmentToMouseVector
						float[] angles = { Vector2.SignedAngle(delimitationVectors[i], segmentToMouseVectors[0]),
										Vector2.SignedAngle(delimitationVectors[i + 1], segmentToMouseVectors[1])};

						// If the mouse inside the delimitations, then the first angle must be negative and the second one positive.
						// Any other result would indicate the mouse is outside.
						if (angles[0] < 0 && angles[1] > 0)
						{
							_firstSegmentPointIndex = i;
							return currentSegmentPoints;
						}
					}

					// If we haven't found any corresponding zone, then we must be outside of an extremity segment
					// so we consider we're still on the same zone as before.

					if (_firstSegmentPointIndex < 0)
						throw new NotImplementedException();

					Vector2[] previousSegmentPositions = {  linePositions[_firstSegmentPointIndex],
														linePositions[_firstSegmentPointIndex + 1] };

					return previousSegmentPositions;
				}

				/// <summary>
				/// Get the precise point in which the resistance must be
				/// given the segment and the mouse position
				/// </summary>
				Vector2 GetPointInSegment()
				{
					var intersections = GetIntersectingPoints();

					var mouseSegmentLength = (intersections[1] - intersections[0]).magnitude;

					var ratio = GetRatioIntoProxySegment();

					// Final point is the interpolation betweent the two segment points by the ratio found above
					var point = Vector2.Lerp(segmentPositions[0], segmentPositions[1], ratio);

					return point;

					/// <summary>
					/// Get the points defining the "proxy segment" line (= the replication of the segment vector at the position)
					/// </summary>
					Vector2[] GetIntersectingPoints()
					{
						// Length of the lines when testing intersections
						float lineLength = 100;

						var segment = (segmentPositions[1] - segmentPositions[0]) * lineLength;

						var intersections = new Vector2[2];

						int i = _firstSegmentPointIndex;

						// Find the points intersecting the delimitation and the the "proxy segment"
						LineUtil.IntersectLineSegments2D(segmentPositions[0] - (delimitationVectors[i] * lineLength),
																			segmentPositions[0] + (delimitationVectors[i] * lineLength),
																			position + segment,
																			position - segment,
																			out intersections[0]);


						LineUtil.IntersectLineSegments2D(segmentPositions[1] - (delimitationVectors[i + 1] * lineLength),
																			segmentPositions[1] + (delimitationVectors[i + 1] * lineLength),
																			position - segment,
																			position + segment,
																			out intersections[1]);

						#region debug

						Debug.DrawLine(position, intersections[0], Color.green);
						Debug.DrawLine(position, intersections[1], Color.blue);
						#endregion

						return intersections;
					}

					/// <summary>
					/// Get how far (in percentage) the mouse is in the proxy segment
					/// </summary>
					float GetRatioIntoProxySegment()
					{
						var lengthIntoProxySegment = (position - intersections[0]).magnitude;

						// If the angle between the delimitation and the segment-to-mouse vector is wrong, then we are at an extremity and must set the ratio to 0;
						var angle = Vector2.SignedAngle(delimitationVectors[_firstSegmentPointIndex], position - segmentPositions[0]);

						if (angle > 0)
							lengthIntoProxySegment = 0;

						var ratio = Mathf.Clamp(lengthIntoProxySegment / mouseSegmentLength, 0, 1);

						return ratio;
					}
				}
			}

			void UnsnapIfTooFar(IPuzzleCircuit circuit, Vector2 position)
			{
				Vector2 currentPosition = transform.position;

				// Difference between the current position and the position in the line
				var diff = position - currentPosition;

				// If difference is higher than a max length, then unsnap the composant
				if (diff.magnitude > DistanceToUnsnap)
				{
					UnSnap();
					base.SetPosition(position);
				}
			}
		}

		/// <summary>
		/// Get the rotation for the composant from the segment slope
		/// </summary>
		protected virtual Vector3 GetRotation(Vector2[] segmentPositions)
		{
			var segment = segmentPositions[1] - segmentPositions[0];

			// The angle of the segment relative to an upward vector, between 0 and 180 (takes the smallest angle)
			var angle = Vector3.Angle(Vector3.up, segment);

			// If the segment vector is pointing to the right, then it's more than 180 degrees and the angle must be calculated accordingly
			if (segment.x > 0)
				angle = 360 - angle;

			var rotation = new Vector3(0, 0, angle);

			return rotation;
		}

		// Get the ratio, between 0 and 1, of how far we are in the line
		protected float RatioInLine()
		{
			if (circuit == null || _firstSegmentPointIndex == -1)
				return -1;

			float lengthToPos = 0.0f;
			float lineLength = circuit.LineLength;

			var linePositions = circuit.LinePositions;

			for (int i = 0; i < _firstSegmentPointIndex; i++)
			{
				lengthToPos += Vector2.Distance(linePositions[i], linePositions[i + 1]);
			}
			lengthToPos += Vector2.Distance(linePositions[_firstSegmentPointIndex], transform.position);

			return lengthToPos / lineLength;
		}

		protected abstract void SetFactors(ref SignalFactors _factors);
	}

	public static class LineUtil
	{
		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T temp = lhs;
			lhs = rhs;
			rhs = temp;
		}

		public static bool Approximately(float a, float b, float tolerance = 1e-5f)
		{
			return Mathf.Abs(a - b) <= tolerance;
		}

		public static float CrossProduct2D(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y;
		}

		/// <summary>
		/// Determine whether 2 lines intersect, and give the intersection point if so.
		/// </summary>
		/// <param name="p1start">Start point of the first line</param>
		/// <param name="p1end">End point of the first line</param>
		/// <param name="p2start">Start point of the second line</param>
		/// <param name="p2end">End point of the second line</param>
		/// <param name="intersection">If there is an intersection, this will be populated with the point</param>
		/// <returns>True if the lines intersect, false otherwise.</returns>
		public static bool IntersectLineSegments2D(Vector2 p1start, Vector2 p1end, Vector2 p2start, Vector2 p2end,
			out Vector2 intersection)
		{
			// Consider:
			//   p1start = p
			//   p1end = p + r
			//   p2start = q
			//   p2end = q + s
			// We want to find the intersection point where :
			//  p + t*r == q + u*s
			// So we need to solve for t and u
			var p = p1start;
			var r = p1end - p1start;
			var q = p2start;
			var s = p2end - p2start;
			var qminusp = q - p;

			float cross_rs = CrossProduct2D(r, s);

			if (Approximately(cross_rs, 0f))
			{
				// Parallel lines
				if (Approximately(CrossProduct2D(qminusp, r), 0f))
				{
					// Co-linear lines, could overlap
					float rdotr = Vector2.Dot(r, r);
					float sdotr = Vector2.Dot(s, r);
					// this means lines are co-linear
					// they may or may not be overlapping
					float t0 = Vector2.Dot(qminusp, r / rdotr);
					float t1 = t0 + sdotr / rdotr;
					if (sdotr < 0)
					{
						// lines were facing in different directions so t1 > t0, swap to simplify check
						Swap(ref t0, ref t1);
					}

					if (t0 <= 1 && t1 >= 0)
					{
						// Nice half-way point intersection
						float t = Mathf.Lerp(Mathf.Max(0, t0), Mathf.Min(1, t1), 0.5f);
						intersection = p + t * r;
						return true;
					}
					else
					{
						// Co-linear but disjoint
						intersection = Vector2.zero;
						return false;
					}
				}
				else
				{
					// Just parallel in different places, cannot intersect
					intersection = Vector2.zero;
					return false;
				}
			}
			else
			{
				// Not parallel, calculate t and u
				float t = CrossProduct2D(qminusp, s) / cross_rs;
				float u = CrossProduct2D(qminusp, r) / cross_rs;
				if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
				{
					intersection = p + t * r;
					return true;
				}
				else
				{
					// Lines only cross outside segment range
					intersection = Vector2.zero;
					return false;
				}
			}
		}
	}

	public static class Vector2Extension
	{
		public static Vector2 Rotate(this Vector2 v, float degrees)
		{
			float radians = degrees * Mathf.Deg2Rad;
			float sin = Mathf.Sin(radians);
			float cos = Mathf.Cos(radians);

			float tx = v.x;
			float ty = v.y;

			return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
		}
	}
}
