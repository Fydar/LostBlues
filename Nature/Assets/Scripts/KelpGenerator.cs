using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpGenerator : MonoBehaviour
{
	[Header("Growth")]
	public float Age;
	public float GrowthAmount = 0.1f;
	public float MaxAge = 12;
	public float GrowthTickTime = 0.5f;
	public AnimationCurve GrowthCurve;

	[Space]
	public float VerticalOffset = 0.5f;

	[Header ("Wiggle")]
	public float WiggleTickTime = 0.25f;
	public float WiggleSpeed = 10;

	public Transform SegmentPrefab;

	private void Start()
	{
		InvokeRepeating ("GrowthTick", GrowthTickTime, GrowthTickTime);
		InvokeRepeating ("WiggleTick", WiggleTickTime, WiggleTickTime);
	}

	private void OnValidate()
	{
		GrowthPropogator (transform, Age);
	}

	private void WiggleTick ()
	{
		WiggleChildren (transform);
	}

	private void GrowthTick()
	{
		Age += GrowthCurve.Evaluate(Age / MaxAge) * GrowthAmount;

		GrowthPropogator (transform, Age);
	}

	private void WiggleChildren(Transform target)
	{
		var child = target.Find("Segment");
		if (child != null)
		{
			child.Rotate (0, 0, Random.Range (-WiggleSpeed, WiggleSpeed) * Time.deltaTime);
			WiggleChildren (child);
		}
	}

	private void GrowthPropogator(Transform target, float size)
	{
		if (!Application.isPlaying)
		{
			return;
		}

		if (size > 1.0f)
		{
			var child = target.Find ("Segment");
			if (child == null)
			{
				child = Instantiate (SegmentPrefab, new Vector3 (0.0f, VerticalOffset, 0.0f), Quaternion.identity, target);
				child.name = SegmentPrefab.name;
			}
			GrowthPropogator (child, size - 1.0f);
			child.localPosition = new Vector3 (0.0f, VerticalOffset, 0.0f);

			float childScale = Mathf.Min (1.0f, size - 1.0f);
			child.localScale = new Vector3 (childScale, childScale, childScale);
		}
		else
		{
			var child = target.Find ("Segment");
			if (child != null)
			{
				Destroy (child.gameObject);
			}
		}
	}
}
