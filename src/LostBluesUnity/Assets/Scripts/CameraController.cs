using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	public float CameraWidth = 8;
	public float CameraHeight = 2;

	public Vector2 VirtualPositon;

	public Rect CameraBounds;

	public Rect InnerBounds;

	private Camera ThisCamera;

	private void Awake()
	{
		Instance = this;
		ThisCamera = GetComponent<Camera> ();

		VirtualPositon = transform.position;
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

		VirtualPositon = new Vector3 (
			Mathf.Clamp(VirtualPositon.x, InnerBounds.xMin, InnerBounds.xMax),
			Mathf.Clamp(VirtualPositon.y, InnerBounds.yMin, InnerBounds.yMax),
			0);


		var totalOffset = Vector2.zero;
		if (PulseEffect.Instances != null)
		{
			foreach (var pulseEffect in PulseEffect.Instances.Values)
			{
				totalOffset += pulseEffect.ShakeOffset;
			}
		}


		var newPosition = VirtualPositon + totalOffset;

		transform.position = new Vector3(newPosition.x, newPosition.y, -10);
	}

	public void SetNormalisedPosition (float x, float y)
	{
		VirtualPositon = new Vector2 (
			Mathf.Lerp(InnerBounds.xMin, InnerBounds.xMax, x),
			Mathf.Lerp (InnerBounds.yMin, InnerBounds.yMax, y));
	}
}
