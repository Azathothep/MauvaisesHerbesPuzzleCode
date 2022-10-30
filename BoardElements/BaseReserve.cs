using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MHPuzzle.Elements;

namespace MHPuzzle.Objects
{
	public class BaseReserve : MonoBehaviour, IPuzzleReserve
	{
		List<IPuzzleElement> reserve = new List<IPuzzleElement>();

		public void Awake()
		{
			var children = GetComponentsInChildren<IPuzzleElement>();
			
			foreach (var child in children)
			{
				reserve.Add(child);
			}
		}

		public void Put(IPuzzleElement o)
		{
			reserve.Add(o);

			var mono = o as MonoBehaviour;

			if (mono == null)
				return;

			mono.transform.SetParent(this.transform);
		}

		public void Take(IPuzzleElement o)
		{
			reserve.Remove(o);
		}
	}
}
