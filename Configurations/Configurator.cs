using System.Collections;
using UnityEngine;

public class Configurator
{
	public void ConfigureInGameObject<T>(T datas, MonoBehaviour obj) where T : struct
	{
		var component = obj.GetComponent< IConfigurable<T> >();
		if (component != null)
			component.Configure(datas);
		else
			Debug.LogWarning($"IConfigurable component not found in {obj}");
	}

	public void ConfigureInChildren<T>(T datas, MonoBehaviour root) where T : struct
	{
		var children = root.GetComponentsInChildren< IConfigurable<T> >();
		foreach (var child in children)
		{
			child.Configure(datas);
		}
	}

	public void ConfigureInContext<T>(T datas, MonoBehaviour obj) where T : struct
	{
		var context = obj.transform.root.GetComponent<MonoBehaviour>();
		ConfigureInChildren(datas, context);
	}
}