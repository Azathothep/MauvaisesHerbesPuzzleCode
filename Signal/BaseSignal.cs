using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public struct _C_WaveDatas
	{
		public int pointNumber;
		public Vector2 xLimits;
		public float speed;

		public _C_WaveDatas(int _pointNumber, Vector2 _xLimits, float _speed)
		{
			pointNumber = _pointNumber;
			xLimits = _xLimits;
			speed = _speed;
		}
	}

	public class BaseSignal : ASignal, IConfigurable<_C_WaveDatas>
	{
		public void Configure(_C_WaveDatas datas)
		{
			_pointNumber = datas.pointNumber;
			_xLimits = datas.xLimits;
			_movementSpeed = datas.speed;
		}
	}
}