using System;
using UnityEngine;
using MHPuzzle.Elements;

namespace MHPuzzle.Impulse
{
	public interface IImpulse
	{
		void Launch(ISignalConnection StartConnection);
	}
}