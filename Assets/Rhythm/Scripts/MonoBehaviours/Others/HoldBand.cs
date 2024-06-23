using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class HoldBand : LaneObject
    {
        private double _beginTime;
        private double _endTime;

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, float length, float beginTime, float endTime, IDisposable disposable)
        {
            transform.localScale = new Vector3(transform.localScale.x, length, transform.localScale.z);
            _beginTime = beginTime;
            _endTime = endTime;
            Create(position, velocity, survivalRect, lane, disposable);
        }
    }
}