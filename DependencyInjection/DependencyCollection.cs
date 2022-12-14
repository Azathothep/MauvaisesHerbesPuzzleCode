using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

// This code was made by following a tutorial on Dependency Injection at https://moderncsharpinunity.github.io/post/dependency-injection-on-unity/

public struct Dependency
{
	public Type Type { get; set; }
	public DependencyFactory.Delegate Factory { get; set; }
	public bool IsSingleton { get; set; }
}

public class DependenciesCollection : IEnumerable<Dependency>
{
	private List<Dependency> dependencies = new List<Dependency>();

	public void Add(Dependency dependency) => dependencies.Add(dependency);

	public IEnumerator<Dependency> GetEnumerator() => dependencies.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => dependencies.GetEnumerator();
}



