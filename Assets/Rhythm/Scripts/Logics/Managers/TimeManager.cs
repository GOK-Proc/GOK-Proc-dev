using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class TimeManager : ITimeProvider
    {
        public double Time { get => _isStartTimer ? UnityEngine.Time.timeAsDouble - _startTime : throw new System.InvalidOperationException(); set => _startTime = UnityEngine.Time.timeAsDouble - value; }

        private double _startTime;
        private bool _isStartTimer;

        public TimeManager()
        {
            _startTime = 0;
            _isStartTimer = false;
        }

        public void StartTimer(double initialTime)
        {
            _startTime = UnityEngine.Time.timeAsDouble - initialTime;
            _isStartTimer = true;
        }

    }
}