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
        private IDisposable _disposable;

        protected Action _onDestroy;

        public bool IsAlive { get; protected set; } 

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, IDisposable disposable)
        {
            _position = position;
            _velocity = velocity;
            _survivalRect = survivalRect;
            _disposable = disposable;

            transform.position = _position;
            IsAlive = true;

            _onDestroy = () =>
            {
                IsAlive = false;
                gameObject.SetActive(false);
                _disposable.Dispose();
            };

            gameObject.SetActive(true);
        }

        protected virtual void Update()
        {
            bool IsOutsideSurvivalRect()
            {
                (var ul, var lr) = _survivalRect;

                if (_position.x < ul.x) return true;

                if (_position.y > ul.y) return true;

                if (_position.x > lr.x) return true;

                if (_position.y < lr.y) return true;

                return false;
            }

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
            _onDestroy?.Invoke();
        }
    }
}