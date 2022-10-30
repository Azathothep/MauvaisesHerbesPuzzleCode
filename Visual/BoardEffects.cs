using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

namespace MHPuzzle.Effects
{
	public static class BoardEffects
	{
		public static IEnumerator Blink<T>(T obj, Color blinkColor, float duration, float speed)
		{
			var blinker = obj as IBlink;
			if (blinker == null)
			{
				Debug.LogWarning($"Unable to blink {obj} : no IBlink found!");
				yield break;
			}

			float delay = 0;

			Renderer rend = blinker.Renderer;

			Color originalColor = rend.material.color;

			rend.material.color = blinkColor;

			for (float time = 0; time < duration; time += Time.deltaTime)
			{
				if (delay > speed)
				{
					if (rend.material.color == originalColor)
						rend.material.color = blinkColor;
					else
						rend.material.color = originalColor;

					delay = 0;
				}
				else
					delay += Time.deltaTime;

				yield return null;
			}

			if (rend.material.color == blinkColor)
			{
				while (delay < speed)
				{
					delay += Time.deltaTime;

					yield return null;
				}
			}

			rend.material.color = originalColor;
		}
	}
}