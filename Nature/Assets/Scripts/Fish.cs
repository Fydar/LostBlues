using UnityEngine;

public class Fish : MonoBehaviour
{
	public float MovementSpeed;

	public float HorizontalFrequency;
	public float HorizontalStrength;

	public float VerticalFrequency;
	public float VerticalStrength;

	private float Channel;

	private Rigidbody2D Body;

	private void Awake ()
	{
		Body = GetComponent<Rigidbody2D> ();
	}

	private void Start ()
	{
		Channel = Random.value;
	}

	private void Update ()
	{
		float horizontal = (Mathf.PerlinNoise (Time.time * HorizontalFrequency, Channel) - 0.5f) * HorizontalStrength * Time.deltaTime;
		float vertical = (Mathf.PerlinNoise (Time.time * VerticalFrequency, Channel) - 0.5f) * VerticalStrength * Time.deltaTime;

		if (Body != null)
		{
			Body.MovePosition (transform.position + new Vector3 (horizontal, vertical, 0));
		}
		else
		{
			transform.position += new Vector3 (horizontal, vertical, 0);
		}
	}
}
