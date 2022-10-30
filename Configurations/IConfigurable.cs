using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IConfigurable<T> where T : struct
{
	public void Configure(T datas);
}