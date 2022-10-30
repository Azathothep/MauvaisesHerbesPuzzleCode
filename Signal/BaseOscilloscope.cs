using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using MHPuzzle.Elements;

namespace MHPuzzle.Signal
{
	public class BaseOscilloscope : AOscilloscope
	{
		[Header("Waves")]

		[SerializeField]
		private ASignal _currentSignal;

		[SerializeField]
		private ASignal _targetSignal;

		protected new void Awake()
		{
			base.Awake();
			CurrentSignal = _currentSignal;
			TargetSignal = _targetSignal;
		}
	}
}