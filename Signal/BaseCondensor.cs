using MHPuzzle.Signal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public class BaseCondensor : AComposant
	{
		protected override void SetFactors(ref SignalFactors _factors)
		{
			if (IsSnapped == false)
				return;

			_factors.amplitude = 1 + RatioInLine();
		}
	}
}
