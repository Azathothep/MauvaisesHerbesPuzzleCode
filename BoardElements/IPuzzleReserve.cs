using MHPuzzle.Elements;
using System.Collections;
using UnityEngine;

namespace MHPuzzle.Objects
{
	public interface IPuzzleReserve
	{
		public void Put(IPuzzleElement o);
		public void Take(IPuzzleElement o);
	}
}