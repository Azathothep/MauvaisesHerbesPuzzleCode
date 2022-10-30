using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

namespace MHPuzzle.Case
{
	public interface IClip
	{
		public bool IsFree { get; }
		void Catch(Vector3 newPos, Transform parent);
		void SetRotation(float z);
		void UnsnapIfTooFar(Vector2 mousePosition);
		void OnGrip();
		void Touch();
		void UnTouch();
	}
}