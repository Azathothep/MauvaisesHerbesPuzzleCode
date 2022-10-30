using System.Collections;
using UnityEngine;

namespace MHPuzzle.Elements
{
	public interface ISnapSpot<T>
	{
		public void Snap(T element);
		public void UnSnap(T element);
		public Vector2 GetPosition();
	}
}