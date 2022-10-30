using System.Collections;
using UnityEngine;
using MHPuzzle.Signal;
using System.Xml.Linq;
using MHPuzzle.Objects;

namespace MHPuzzle.Elements
{
	public abstract class ABaseElement : IsDraggable, IPuzzleElement
	{
		public ElementType Type { get => _type; }

		[SerializeField]
		private ElementType _type;

		public bool IsBroken { get => _isBroken; }

		[SerializeField]
		private bool _isBroken = false;

		[SerializeField]
		[Range(0.0f, 10.0f)] private float _distanceToUnsnap;

		public IPuzzleEmplacement Emplacement { get; private set; }

		public ISignalConnection[] Connections { get; private set; }

		public SignalProperties TargetSignal { get => _targetSignal; }

		[SerializeField]
		private SignalProperties _targetSignal;

		public bool IsSnapped { get; private set; }

		protected new void Awake()
		{
			base.Awake();
			IsSnapped = false;
		}

		public override void SetPosition(Vector2 mousePosition)
		{
			if (IsSnapped == true)
				UnsnapIfTooFar();

			if (IsSnapped == false)
				base.SetPosition(mousePosition);

			void UnsnapIfTooFar()
			{
				Vector2 position = transform.position;

				// Difference between the current position and the position in the line
				var diff = position - mousePosition;

				// If difference is higher than a max length, then unsnap the composant
				if (diff.magnitude > _distanceToUnsnap)
				{
					UnSnap();
				}
			}
		}

		public void Snap(IPuzzleEmplacement spot)
		{
			Emplacement = spot;
			SetPosition(Emplacement.GetPosition());
			Connections = spot.Connections;
			IsSnapped = true;
		}

		public void UnSnap()
		{
			Emplacement.UnSnap(this);
			Emplacement = null;
			Connections = null;
			IsSnapped = false;
		}
	}
}