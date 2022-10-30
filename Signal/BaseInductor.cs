using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public class BaseInductor : AComposant
	{
		private float _prevRatio = 0.0f;

		[SerializeField] private float _rotationSpeed;

		protected override void SetFactors(ref SignalFactors _factors)
		{
			if (IsSnapped == false)
				return;

			_factors.frequency = RatioInLine();
		}

		protected override Vector3 GetRotation(Vector2[] segmentPositions)
		{
			var angles = transform.eulerAngles;

			if (_prevRatio < RatioInLine())
				angles.z -= _rotationSpeed; // find right direction for angles
			else if (_prevRatio > RatioInLine())
				angles.z += _rotationSpeed;

			_prevRatio = RatioInLine();

			return angles;
		}
	}
}