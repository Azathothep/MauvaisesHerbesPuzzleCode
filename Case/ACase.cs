using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHPuzzle.Case
{
	public static class GaugeFiller
	{
		public static IEnumerator Launch(AGauge gauge, float delay)
		{
			float time = 0;

			while (time < delay)
			{
				if (Input.GetMouseButton(0) == false)
				{
					gauge.Stop();
					yield break;
				}

				time += Time.deltaTime;
				gauge.Fill(time / delay);

				yield return null;
			}

			gauge.Stop();
		}
	}

	public static class SpriteEffects
	{
		public static IEnumerator Disappear(SpriteRenderer sprite, float delay)
		{
			float time = delay;

			while (time > 0)
			{
				time -= Time.deltaTime;

				SetAlpha(sprite, time / delay);

				yield return null;
			}

			SetAlpha(sprite, 0);
		}

		public static void SetAlpha(SpriteRenderer sprite, float a)
		{
			var c = sprite.material.color;
			c.a = a;
			sprite.material.color = c;
		}
	}

	public abstract class ACase : MonoBehaviour, IClickable, ICase, IHoverable
	{
		public bool IsSelectable { get; set; }

		public bool IsClickable { get; private set; }

		[SerializeField] private float clickTimeToRemove;

		[SerializeField] private AGauge gauge;

		[SerializeField] private AHoverable sprite;

		private List<IClip> clips;

		private bool IsFree = false;

		[Header("Disappearance")]

		[SerializeField] private float disappearDelay;

		private void Awake()
		{
			IsSelectable = true;

			IsClickable = true;

			var clipArray = GetComponentsInChildren<IClip>();

			clips = new List<IClip>();

			foreach (var clip in clipArray)
				clips.Add(clip);
		}

		private void Free() => IsFree = true;

		public void Unty(IClip clip)
		{
			clips.Remove(clip);
			if (clips.Count == 0)
				Free();
		}

		public void OnClick()
		{
			if (IsFree == false)
				return;

			StopAllCoroutines();
			StartCoroutine(OnClickInternal());

			IEnumerator OnClickInternal()
			{
				yield return StartCoroutine(GaugeFiller.Launch(gauge, clickTimeToRemove));

				sprite.UnHover();

				sprite.IsSelectable = false;

				yield return StartCoroutine(SpriteEffects.Disappear(sprite.GetComponent<SpriteRenderer>(), disappearDelay));

				this.gameObject.SetActive(false);
			}
		}

		public void Hover() => sprite.Hover();

		public void UnHover() => sprite.UnHover();
	}
}