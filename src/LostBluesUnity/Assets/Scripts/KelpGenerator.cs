using UnityEngine;

namespace LostBluesUnity
{
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

        public Transform SegmentPrefab;

        private void Start()
        {
            InvokeRepeating("GrowthTick", GrowthTickTime, GrowthTickTime);

            GrowthUpdate();
        }

        private void GrowthTick()
        {
            Age += GrowthCurve.Evaluate(Age / MaxAge) * GrowthAmount;

            GrowthUpdate();
        }

        public void GrowthUpdate()
        {
            GrowthPropagator(transform, Age);
        }

        private void GrowthPropagator(Transform target, float size, int index = 0)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (index == 0)
            {
                float thisScale = Mathf.Min(1.0f, size - 1.0f);
                target.localScale = new Vector3(thisScale, thisScale, thisScale);
            }

            if (size > 1.0f)
            {
                var child = target.Find("Segment");
                if (child == null)
                {
                    child = Instantiate(SegmentPrefab, new Vector3(0.0f, VerticalOffset, 0.0f), Quaternion.identity, target);
                    child.name = SegmentPrefab.name;

                    var childSegment = child.GetComponent<Segment>();
                    childSegment.Generator = this;
                    childSegment.SegmentNumber = index;
                }
                GrowthPropagator(child, size - 1.0f, index + 1);

                child.localPosition = index != 0
                    ? new Vector3(0.0f, VerticalOffset, 0.0f)
                    : Vector3.zero;

                float childScale = Mathf.Min(1.0f, size - 1.0f);
                child.localScale = new Vector3(childScale, childScale, childScale);
            }
            else
            {
                var child = target.Find("Segment");
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
