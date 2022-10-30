using MHPuzzle.Signal;
using System.Collections;
using UnityEngine;
using System;
using MHPuzzle.Elements;
using MHPuzzle.Objects;

namespace MHPuzzles
{
	public class PuzzleDependenciesContext : DependenciesContext
	{
		[SerializeField]
		private AOscilloscope oscilloscope;

		[SerializeField]
		private BaseMotherBoard motherBoard;

		[SerializeField]
		private BaseReserve reserve;

		protected override void Setup()
		{
			dependenciesCollection.Add(NewDependency(typeof(Configurator), DependencyFactory.FromClass<Configurator>(), true));
			dependenciesCollection.Add(NewDependency(typeof(AOscilloscope), DependencyFactory.FromGameObject(oscilloscope)));
			dependenciesCollection.Add(NewDependency(typeof(IMotherBoard), DependencyFactory.FromGameObject(motherBoard)));
			dependenciesCollection.Add(NewDependency(typeof(IPuzzleReserve), DependencyFactory.FromGameObject(reserve)));
		}

		protected override void Configure()
		{
			
		}

		Dependency NewDependency(Type type, DependencyFactory.Delegate factory, bool isSingleton = false)
		{
			return new Dependency { Type = type, Factory = factory, IsSingleton = isSingleton };
		}
	}
}