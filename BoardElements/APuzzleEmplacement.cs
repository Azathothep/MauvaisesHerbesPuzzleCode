using MHPuzzle.Objects;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace MHPuzzle.Elements
{
	public abstract class APuzzleEmplacement : MonoBehaviour, IPuzzleEmplacement
	{
		[SerializeField]
		private bool Verbose;

		public ElementType Type { get => _type; }

		[SerializeField]
		private ElementType _type;

		[InjectField]
		private IPuzzleReserve reserve;

		public UnityEvent OnElementSnap { get => _onElementSnap; }

		private UnityEvent _onElementSnap = new UnityEvent();

		public IPuzzleElement Element { get; private set; }

		public ISignalConnection[] Connections { get; private set; }

		public Vector2 GetPosition() => transform.position;

		public virtual void Snap(IPuzzleElement element)
		{
			reserve.Take(element);

			Element = element;
			Element.Snap(this);

			var mono = element as MonoBehaviour;
			if (mono)
				mono.transform.SetParent(this.transform);

			OnElementSnap.Invoke();
		}

		public virtual void UnSnap(IPuzzleElement element)
		{
			Element = null;

			reserve.Put(element);
		}

		//

		protected void Awake()
		{
			Connections = GetComponentsInChildren<ISignalConnection>();
			
			if (Verbose)
				Debug.Log($"Found {Connections.Length} PuzzleConnections");

			foreach (ISignalConnection c in Connections)
				c.Emplacement = this;
		}

		private IPuzzleElement GetElement(GameObject o) => o.GetComponent<IPuzzleElement>();

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (Element != null)
				return;

			var e = GetElement(other.gameObject);

			if (e == null || e.Type != Type)
				return;

			Snap(e);
		}
	}
}