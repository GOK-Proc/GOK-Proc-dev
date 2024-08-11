using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class AccelerateObject : RhythmGameObject
    {
        private Vector3 _acceleration;

        public void Create(Vector3 position, Vector3 velocity, Vector3 acceleration, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, IDisposable disposable)
        {
            Create(position, velocity, acceleration, survivalRect, null, disposable);
        }

        public void Create(Vector3 position, Vector3 velocity, Vector3 acceleration, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, Action<Action> onCompleted, IDisposable disposable)
        {
            _acceleration = acceleration;
            Create(position, velocity, survivalRect, onCompleted, disposable);
        }

        protected override void Update()
        {
            _velocity += _acceleration * Time.deltaTime;
            base.Update();
        }
    }
}