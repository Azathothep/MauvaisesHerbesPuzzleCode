using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public class BaseResistance : AComposant
	{
		protected override void SetFactors(ref SignalFactors _factors)
		{
			if (IsSnapped == false)
				return;

			_factors.amplitude = 1 - RatioInLine();
		}
	}
}