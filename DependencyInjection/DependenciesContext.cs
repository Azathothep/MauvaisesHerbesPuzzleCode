using System.Collections;
using UnityEngine;

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