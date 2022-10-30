using System.Collections;
using UnityEngine;

namespace MHPuzzle.Elements
{
	public interface ISnappable<T>
	{
		public bool IsSnapped { get; }

		public void Snap(T spot);
		public void UnSnap();
	}
}