using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public float CameraWidth = 8;
	public float CameraHeight = 2;

	public Rect CameraBounds;

	public Rect InnerBounds;

	private Camera ThisCamera;

	private void Awake()
	{
		ThisCamera = GetComponent<Camera> ();
	}

	private void Update ()
	{
		float ratio = ((float)Screen.height) / Screen.width;

		CameraWidth = ThisCamera.orthographicSize / ratio;
		CameraHeight = CameraWidth * ratio;

		float innerBoundsHeight = Mathf.Max (0, CameraBounds.height - (CameraHeight * 2));
		float innerBoundsWidth = Mathf.Max (0, CameraBounds.width - (CameraWidth * 2));

		InnerBounds = new Rect(Mathf.Min (0, CameraBounds.xMin + CameraWidth),
			Mathf.Min(0, CameraBounds.yMin + CameraHeight),
			innerBoundsWidth,
			innerBoundsHeight);

		transform.position = new Vector3 (
			Mathf.Clamp(transform.position.x, InnerBounds.xMin, InnerBounds.xMax),
			Mathf.Clamp(transform.position.y, InnerBounds.yMin, InnerBounds.yMax),
			0);
	}

	public void SetNormalisedPosition (float x, float y)
	{
		transform.position = new Vector3 (
			Mathf.Lerp(InnerBounds.xMin, InnerBounds.xMax, x),
			Mathf.Lerp (InnerBounds.yMin, InnerBounds.yMax, y),
			0);
	}
}
