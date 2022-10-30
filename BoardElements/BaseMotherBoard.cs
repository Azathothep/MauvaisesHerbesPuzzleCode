using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MHPuzzle.Objects;
using System.Linq;

namespace MHPuzzle.Elements
{
	public class BaseMotherBoard : MonoBehaviour, IMotherBoard
	{
		[SerializeField]
		private bool Verbose = false;

		public IPuzzleElement[] elements { get; private set; }

		private Dictionary<IPuzzleElement, bool> elementsTouched;

		private void Awake()
		{
			RegisterElements();
		}

		public void Reload()
		{
			RegisterElements();

			ResetTouch();

			ForceComputeCircuit();
		}

		private void RegisterElements()
		{
			elements = GetComponentsInChildren<IPuzzleElement>();

			elementsTouched = new Dictionary<IPuzzleElement, bool>();

			foreach (var element in elements)
			{
				elementsTouched.Add(element, false);
			}
		}

		private void ResetTouch()
		{
			var keys = elementsTouched.Keys;

			foreach (var k in keys.ToArray())
			{
				elementsTouched[k] = false;
			}
		}

		private void ForceComputeCircuit()
		{
			var circuits = GetComponentsInChildren<IPuzzleCircuit>();

			foreach (var circuit in circuits)
				circuit.ApplyFactors();
		}

		public bool IsEveryElementConnected() => !elementsTouched.ContainsValue(false);

		public void Touch(IPuzzleElement element)
		{
			if (element == null || elementsTouched.ContainsKey(element) == false)
			{
				if (Verbose) Debug.LogWarning($"Trying to touch unregistered element : {element}");
				return;
			}

			elementsTouched[element] = true;
		}
	}
}