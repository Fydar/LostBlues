using System.Collections;
using UnityEngine;

public class IntroSequence : MonoBehaviour
{
	[Header ("Calm Fade")]
	public float SequenceDuration;

	[Space]
	public CanvasGroup WhiteFade;
	public AnimationCurve WhiteFadeCurve;

	[Space]
	public CameraController CameraPan;
	public AnimationCurve CameraPanCurve;

	private IEnumerator Start ()
	{
		foreach (float time in new TimedLoop (SequenceDuration))
		{
			WhiteFade.alpha = 1.0f - WhiteFadeCurve.Evaluate (time);
			CameraPan.SetNormalisedPosition (0.5f, 1.0f - CameraPanCurve.Evaluate (time));

			yield return null;
		}

		while (true)
		{
			CameraPan.SetNormalisedPosition (0.5f, 0.0f);
			yield return null;
		}
	}
}
