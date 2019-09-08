using UnityEngine;

public class PerlinShake : MonoBehaviour
{
	public float duration = 0.5f;
	public float speed = 1.0f;
	public float magnitude = 0.1f;

	public AnimationCurve falloff;
	
	private float elapsed;
	private float intencity;

	public bool PlayOnStart;
	public float StartIntencity;

	private void Awake ()
	{
		intencity = 0.0f;
		elapsed = 1000000;
	}

	private void Start()
	{
		if (PlayOnStart)
		{
			PlayShake (StartIntencity);
		}
	}

	public void PlayShake (float _intencity)
	{
		intencity = _intencity;
		elapsed = 0;
	}

	private void Update ()
	{
		elapsed += Time.deltaTime;

		float percentComplete = elapsed / duration;
		percentComplete = falloff.Evaluate (percentComplete);

		float damper = 1.0f - Mathf.Clamp (2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

		float alpha = speed * percentComplete * intencity;

		float x = Mathf.PerlinNoise (alpha, 0) * 2.0f - 1.0f;
		float y = Mathf.PerlinNoise (0, alpha) * 2.0f - 1.0f;

		x *= magnitude * damper * intencity;
		y *= magnitude * damper * intencity;

		transform.localPosition = new Vector3 (x, y, transform.localPosition.z);
	}
}
