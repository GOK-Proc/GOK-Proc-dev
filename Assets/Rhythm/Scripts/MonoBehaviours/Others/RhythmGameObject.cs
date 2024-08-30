using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObject : MonoBehaviour
    {
        protected Vector3 _position;
        protected Vector3 _velocity;

        private (Vector2 UpperLeft, Vector2 LowerRight) _survivalRect;

        private Action _destroyer;
        private Action<Action> _onCompleted;

        public bool IsAlive { get; protected set; } 

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, IDisposable disposable)
        {
            Create(position, velocity, survivalRect, null, disposable);
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, Action<Action> onCompleted, IDisposable disposable)
        {
            _position = position;
            _velocity = velocity;
            _survivalRect = survivalRect;

            transform.position = _position;
            IsAlive = true;

            _destroyer = () =>
            {
                gameObject.SetActive(false);
                disposable.Dispose();
            };

            _onCompleted = onCompleted;
            gameObject.SetActive(true);
        }

        public void AdjustPosition(double difference)
        {
            _position += _velocity * (float)difference;
            transform.position = _position;
        }

        protected bool IsOutsideSurvivalRect()
        {
            (var ul, var lr) = _survivalRect;

            if (_position.x < ul.x) return true;

            if (_position.y > ul.y) return true;

            if (_position.x > lr.x) return true;

            if (_position.y < lr.y) return true;

            return false;
        }

        protected virtual void Update()
        {
            if (IsAlive)
            {
                _position += _velocity * Time.deltaTime;
                transform.position = _position;

                if (IsOutsideSurvivalRect())
                {
                    Destroy();
                }
            }
        }

        protected void Destroy()
        {
            IsAlive = false;

            if (_onCompleted is not null)
            {
                _onCompleted.Invoke(_destroyer);
            }
            else
            {
                _destroyer?.Invoke();
            }
        }
    }
}