using UnityEngine;

namespace LostBluesUnity
{
    public class Fish : MonoBehaviour
    {
        public float MovementSpeed;

        public float HorizontalFrequency;
        public float HorizontalStrength;

        public float VerticalFrequency;
        public float VerticalStrength;

        private Vector2 HorizontalRandomOffset;
        private Vector2 VerticalRandomOffset;

        private Rigidbody2D Body;

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            HorizontalRandomOffset = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
            VerticalRandomOffset = new Vector2(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
        }

        private void Update()
        {
            float horizontal = (Mathf.PerlinNoise((Time.time * HorizontalFrequency) + HorizontalRandomOffset.x, HorizontalRandomOffset.y) - 0.5f) * HorizontalStrength * Time.deltaTime;
            float vertical = (Mathf.PerlinNoise((Time.time * VerticalFrequency) + VerticalRandomOffset.x, VerticalRandomOffset.y) - 0.5f) * VerticalStrength * Time.deltaTime;

            var velocity = new Vector3(horizontal, vertical, 0);

            transform.localScale = new Vector3(Mathf.Sign(velocity.x), 1);

            if (Body != null)
            {
                Body.MovePosition(transform.position + velocity);
            }
            else
            {
                transform.position += velocity;
            }

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, CameraController.Instance.CameraBounds.xMin - 0.3f, CameraController.Instance.CameraBounds.xMax + 0.3f),
                Mathf.Clamp(transform.position.y, CameraController.Instance.CameraBounds.yMin + 1.8f, CameraController.Instance.CameraBounds.yMax),
                transform.position.z);
        }
    }
}
