using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
	[SerializeField] IInput input;

	public void Constructor(IInput _input)
	{
		input = _input;

		input.Enable();
	}

    void Update()
    {
		input?.ReadInput();
    }
}
