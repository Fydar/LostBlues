using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
	public int MaxHealth = 4;
	public int Health;

	public KelpGenerator Generator;
	public int SegmentNumber;

	[Header ("Wiggle")]
	public float WiggleTickTime = 0.25f;
	public float WiggleSpeed = 10;

	public float HitWiggle = 25;

	private void Start()
	{
		Health = MaxHealth;

		InvokeRepeating ("WiggleTick", WiggleTickTime, WiggleTickTime);
	}

	private void WiggleTick ()
	{
		transform.Rotate (0, 0, Random.Range (-WiggleSpeed, WiggleSpeed) * Time.deltaTime);
	}

	public void CutAt()
	{
		Health -= StoreUpgrade.Instance.CurrentDamage;

		transform.Rotate (0, 0, Random.Range (-HitWiggle, HitWiggle));

		if (Health <= 0)
		{
			float newAge = SegmentNumber + 1.5f;
			float difference = Generator.Age - newAge;

			Game.Instance.Currency.Value += Mathf.FloorToInt(difference);

			Generator.Age = newAge;
			Generator.GrowthUpdate ();
			Health = MaxHealth;
		}
	}
}
