﻿using System.Collections;
using System.Collections.Generic;

namespace LostBluesUnity
{
    /// <summary>
    /// Used for creating quick and easy loops inside a coroutine for an amount of seconds.
    /// </summary>
    /// <example>
    /// TimedLoop timer = new TimedLoop (0.5f);
    ///
    ///	foreach (float perc in timer)
    ///	{
    ///		Holder.pivot = Vector2.Lerp (fromPivot, toPivot, perc);
    ///
    ///		// Optionally
    ///
    ///		timer.Reset ();
    ///		timer.Break ();
    ///		timer.End ();
    ///
    ///		yield return null;
    /// }
    /// </example>
    public sealed class TimedLoop : IEnumerator<float>, IEnumerable<float>
    {
        private float duration;
        private bool endNext;

        public float Current => Percent;

        public float Duration
        {
            get => duration;
            set
            {
                duration = value;

                if (Time > duration)
                {
                    Time = duration;
                    endNext = true;
                }
            }
        }

        public float Time { get; set; }

        public float Percent
        {
            get => Time / duration;
            set => Time = duration * value;
        }

        IEnumerator<float> IEnumerable<float>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        object IEnumerator.Current => Percent;

        public TimedLoop(float _duration)
        {
            Time = 0.0f;
            duration = _duration;
            endNext = false;
        }

        public void End()
        {
            Time = duration;
        }

        public void Break()
        {
            Time = duration;
            endNext = true;
        }

        public bool MoveNext()
        {
            Time += UnityEngine.Time.deltaTime;

            if (Time < duration)
            {
                return true;
            }

            if (endNext == false)
            {
                endNext = true;
                Time = duration;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            Time = 0.0f;
            endNext = false;
        }

        public void Dispose()
        {
        }
    }
}
