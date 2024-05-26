using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObject : MonoBehaviour, IMovable, IDestroyable
    {
        protected Vector3 _position;
        protected Vector3 _velocity;

        protected ITimeProvider _timeProvider;

        private double _lastTime;
        private float DeltaTime { get => (float)(_timeProvider.Time - _lastTime); }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void Create(Vector3 position, Vector3 velocity)
        {
            _position = position;
            _velocity = velocity;
            _lastTime = _timeProvider.Time;

            transform.position = _position;
            gameObject.SetActive(true);
        }

        // Call this method once per frame
        public void Move()
        {
            _position += _velocity * DeltaTime;
            _lastTime = _timeProvider.Time;
            transform.position = _position;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}