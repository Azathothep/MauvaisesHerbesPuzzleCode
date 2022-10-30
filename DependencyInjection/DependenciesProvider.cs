using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// This code was made by following a tutorial on Dependency Injection at https://moderncsharpinunity.github.io/post/dependency-injection-on-unity/

public class DependenciesProvider
{
	private Dictionary<Type, Dependency> dependencies = new Dictionary<Type, Dependency>();
	private Dictionary<Type, object> singletons = new Dictionary<Type, object>();

	public DependenciesProvider(DependenciesCollection dependencies)
	{
		foreach (var dependency in dependencies)
		{
			this.dependencies.Add(dependency.Type, dependency);
		}
	}

	public object Get(Type type)
	{
		if (!dependencies.ContainsKey(type))
			throw new ArgumentException("Type is not a dependency: " + type.FullName);

		var dependency = dependencies[type];
		if (dependency.IsSingleton)
		{
			if (!singletons.ContainsKey(type))
				singletons.Add(type, dependency.Factory(this));
			return singletons[type];
		}
		else
		{
			return dependency.Factory(this);
		}
	}

	public T Get<T>()
	{
		return (T)Get(typeof(T));
	}

	public object Inject(object dependant)
	{
		Type type = dependant.GetType();

		while (type != null)
		{
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

			foreach (var field in fields)
			{
				if (field.GetCustomAttribute<InjectFieldAttribute>(false) == null) { continue; }

				field.SetValue(dependant, Get(field.FieldType));
			}
			type = type.BaseType;
		}
		return dependant;
	}
}