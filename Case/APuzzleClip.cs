using System.Collections;
using UnityEngine;

namespace MHPuzzle.Case
{
	public abstract class APuzzleClip : IsDraggable, IClip
	{
		public bool IsFree { get; private set; }

		[SerializeField]
		private float distanceToUnsnap = 10.0f;

		private ICase puzzleCase;

		[SerializeField]
		private ParticleSystem poofParticle;

		private AHoverable m_hover;

		private SpriteRenderer m_clipRenderer;

		private bool IsCatched = false;

		private Vector3 SnapPos;

		private new void Awake()
		{
			base.Awake();

			puzzleCase = GetComponentInParent<ICase>();
			m_clipRenderer = GetComponent<SpriteRenderer>();
			m_hover = GetComponent<IsHoverable>();

			IsFree = false;
		}

		public void Catch(Vector3 newPos, Transform newParent)
		{
			transform.SetParent(newParent);
			transform.localPosition = newPos;
			SnapPos = newPos;
			IsCatched = true;
		}

		public override void UnSelect()
		{
			base.UnSelect();

			if (IsFree && IsCatched)
				transform.localPosition = SnapPos;
		}

		public override void SetPosition(Vector2 position)
		{
			if (IsFree)
				base.SetPosition(position);
		}

		public void SetRotation(float z)
		{
			var r = transform.rotation;
			var a = r.eulerAngles;
			a.z = z;
			r.eulerAngles = a;
			transform.rotation = r;
		}

		private void Free()
		{
			IsFree = true;
			puzzleCase.Unty(this);
			poofParticle.Play();		
		}

		public void UnsnapIfTooFar(Vector2 mousePosition)
		{
			Vector2 position = transform.position;

			// Difference between the current position and the position in the line
			var diff = position - mousePosition;

			// If difference is higher than a max length, then unsnap the composant
			if (diff.magnitude > distanceToUnsnap)
			{
				Free();
				base.SetPosition(mousePosition);
			}
		}

		public void OnGrip() => m_hover.UnHover();

		public void Touch() => m_hover.Hover();

		public void UnTouch() => m_hover.UnHover();
	}
}