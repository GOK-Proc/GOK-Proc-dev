using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class TimeManager : ITimeProvider
    {
        public double Time { get => UnityEngine.Time.realtimeSinceStartupAsDouble - _startTime; }

        private readonly double _startTime;

        public TimeManager()
        {
            _startTime = UnityEngine.Time.realtimeSinceStartupAsDouble;
        }

    }
}