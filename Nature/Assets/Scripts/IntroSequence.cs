using System.Collections;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{

	public AudioSource Seagulls;

	[Header("Calm Fade")]
	public float SequenceDuration;

	[Space]
	public CanvasGroup WhiteFade;
	public AnimationCurve WhiteFadeCurve;

	[Space]
	public CameraController CameraPan;
	public AnimationCurve CameraPanCurve;

	[Space]
	public float CalmDelay = 2;

	[Header ("Trawling Net")]
	public float TrawlingNetDuration = 6;
	public float TrawlingDistance = 24;
	public Rigidbody2D TrawlingNet;
	public AnimationCurve TrawlingNetCurve;
	public AnimationCurve CameraShakeIntencity;

	[Space]
	public AnimationCurve TrawlingBoatAudio;
	public AnimationCurve TrawlingBoatAudioCurve;

	private IEnumerator Start ()
	{
		if (Seagulls != null)
		{
			Seagulls.Play();
		}

		foreach (var time in new TimedLoop (SequenceDuration))
		{
			WhiteFade.alpha = 1.0f - WhiteFadeCurve.Evaluate (time);
			CameraPan.SetNormalisedPosition (0.5f, 1.0f - CameraPanCurve.Evaluate (time));

			yield return null;
		}

		yield return new WaitForSeconds (CalmDelay);

		foreach (var time in new TimedLoop (TrawlingNetDuration))
		{
			TrawlingNet.MovePosition(new Vector3 (Mathf.Lerp (-TrawlingDistance, TrawlingDistance, TrawlingNetCurve.Evaluate(time)),
				TrawlingNet.transform.position.y,
				TrawlingNet.transform.position.z));

			yield return null;
		}

	}
}
