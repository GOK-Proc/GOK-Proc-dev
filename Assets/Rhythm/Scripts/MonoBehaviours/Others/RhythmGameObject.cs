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

        private Vector3 _border;
        private IDisposable _disposable;

        public bool IsAlive { get; private set; } 

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Create(Vector3 position, Vector3 velocity, Vector3 border, IDisposable disposable)
        {
            _position = position;
            _velocity = velocity;
            _border = border;
            _disposable = disposable;

            transform.position = _position;
            IsAlive = true;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            bool IsEndPoint()
            {
                if (_velocity.x >= 0)
                {
                    if (_position.x < _border.x) return false;
                }
                else
                {
                    if (_position.x > _border.x) return false;
                }
                if (_velocity.y >= 0)
                {
                    if (_position.y < _border.y) return false;
                }
                else
                {
                    if (_position.y > _border.y) return false;
                }
                if (_velocity.z >= 0)
                {
                    if (_position.z < _border.z) return false;
                }
                else
                {
                    if (_position.z > _border.z) return false;
                }

                return true;
            }

            if (IsAlive)
            {
                _position += _velocity * Time.deltaTime;
                transform.position = _position;

                if (IsEndPoint())
                {
                    Destroy();
                }
            }
        }

        protected void Destroy()
        {
            IsAlive = false;
            gameObject.SetActive(false);
            _disposable.Dispose();
        }
    }
}