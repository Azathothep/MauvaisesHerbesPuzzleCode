using System.Collections;
using UnityEngine;
using Yarn.Unity;

namespace MHPuzzle.Elements
{
	public interface IMotherBoard
	{
		IPuzzleElement[] elements { get; }
		void Touch(IPuzzleElement element);
		bool IsEveryElementConnected();
		void Reload();
	}
}