using System.Collections;
using UnityEngine;

// This code was made following a tutorial on Dependency Injection at https://moderncsharpinunity.github.io/post/dependency-injection-on-unity/

[DefaultExecutionOrder(-1)]
public abstract class DependenciesContext : MonoBehaviour
{
	protected DependenciesCollection dependenciesCollection = new DependenciesCollection();
	private DependenciesProvider dependenciesProvider;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Setup();

		dependenciesProvider = new DependenciesProvider(dependenciesCollection);

		var children = GetComponentsInChildren<MonoBehaviour>(true);
		foreach (var child in children)
		{
			dependenciesProvider.Inject(child);
		}

		Configure();
	}

	protected abstract void Setup();

	protected abstract void Configure();
}