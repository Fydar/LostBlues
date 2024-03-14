using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
	public static Game Instance;

	public Camera SceneCamera;
	public PixelTerrainGenerator Terrain;

	[Header("Cooldown")]
	public float CooldownDuration = 0.25f;
	public float CooldownPercentage;
	public Image BarFill;

	[Header ("Currency")]
	public float CurrentAnimateTime = 0.5f;

	public int CurrentRenderedValue;
	public int OldValue;

	public Text CurrencyText;
	public AudioSource CurrencyGainSound;
	public EventField<int> Currency = new EventField<int> ();
	private Coroutine AnimateUp;

	public float PlaceHeight = 0.5f;

	public bool IsOnCooldown
	{
		get
		{
			return CooldownPercentage > 0.0f;
		}
	}

	private void Awake()
	{
		Instance = this;

		Currency.onChanged += () =>
		{
			if (AnimateUp != null)
			{
				StopCoroutine (AnimateUp);
			}
			OldValue = CurrentRenderedValue;
			AnimateUp = StartCoroutine (AnimateUpCoroutine());
		};
	}

	IEnumerator AnimateUpCoroutine()
	{
		foreach (var time in new TimedLoop(CurrentAnimateTime))
		{
			int nextValue = (int)Mathf.Lerp (OldValue, Currency.Value, time);

			if (CurrentRenderedValue != nextValue)
			{
				CurrencyText.text = CurrentRenderedValue.ToString ();

				if (nextValue > CurrentRenderedValue)
				{
					CurrencyGainSound.Play ();
				}
				CurrentRenderedValue = nextValue;
			}
			yield return null;
		}
		CurrencyText.text = Currency.Value.ToString ();
	}

	private void Update ()
	{
		if (CooldownPercentage > 0.0f)
		{
			CooldownPercentage -= Time.deltaTime / CooldownDuration;
			BarFill.fillAmount = 1.0f - CooldownPercentage;
		}
	}

	public void UiClickAnywhere ()
	{
		var ray = SceneCamera.ScreenPointToRay (Input.mousePosition);

		var scenePoint = ray.origin + (ray.direction * 10);

		var cast = Physics2D.CircleCast (ray.origin, 0.1f, Vector2.zero);

		if (StoreItem.CurrentlySelected != null)
		{
			if (StoreItem.CurrentlySelected.InisideTerrain)
			{
				if (cast.transform != null && cast.transform.name == "FloorCollider")
				{
					PulseEffect.Instances["PlaceBubbles"].PlayAt (scenePoint);

					var clone = Instantiate (StoreItem.CurrentlySelected.Prefab[Random.Range(0, StoreItem.CurrentlySelected.Prefab.Length)]);

					clone.transform.position = new Vector3 (scenePoint.x, PlaceHeight, 10);

					Currency.Value -= StoreItem.CurrentlySelected.CostAmount;
					return;
				}
			}
			else
			{
				PulseEffect.Instances["PlaceBubbles"].PlayAt (scenePoint);

				var clone = Instantiate (StoreItem.CurrentlySelected.Prefab[Random.Range (0, StoreItem.CurrentlySelected.Prefab.Length)]);

				clone.transform.position = new Vector3 (scenePoint.x, scenePoint.y, 10);

				Currency.Value -= StoreItem.CurrentlySelected.CostAmount;
				return;
			}

			StoreItem.CurrentlySelected.Deselect ();
		}

		if (IsOnCooldown)
		{
			return;
		}

		CooldownPercentage = 1.0f;

		if (cast.transform != null)
		{
			var segment = cast.transform.GetComponent<Segment> ();

			if (segment != null)
			{
				segment.CutAt ();
				PulseEffect.Instances["CutBubbles"].PlayAt (scenePoint);
				return;
			}
		}

		PulseEffect.Instances["CutMiss"].PlayAt (scenePoint);
	}
}
