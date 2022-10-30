using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public interface IOscilloscope
	{
		public void DisplayWaves(SignalProperties current, SignalProperties target);
	}
}
