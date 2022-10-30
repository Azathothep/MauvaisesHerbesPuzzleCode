using System.Collections;
using UnityEngine;

namespace MHPuzzle.Signal
{
	public struct WaveSprite
	{
		public Sprite sprite;
	}

	public class BaseTransistor : AComposant, IConfigurable<WaveSprite>
	{
		[Header("Transistor")]

		[SerializeField] private SignalType _type;

		[SerializeField] private SpriteRenderer _waveRenderer;

		public SignalType Type { get => _type; private set { _type = value; } }

		public void Configure(WaveSprite waveSprite)
		{
			_waveRenderer.sprite = waveSprite.sprite;
		}

		protected override void SetFactors(ref SignalFactors _factors)
		{
			if (IsSnapped == false)
				return;

			if (_factors.signalBlend != null)
			{
				_factors.signalBlend.type = _type;
				_factors.signalBlend.blend = RatioInLine();
			}
			else
				_factors.signalBlend = new SignalBlend(_type, RatioInLine());
		}
	}
}