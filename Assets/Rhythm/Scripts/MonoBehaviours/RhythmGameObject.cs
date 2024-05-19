using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObject : MonoBehaviour, IMovable, IDestroyable
    {
        protected Vector3 _position;
        protected Vector3 _velocity;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Create(Vector3 position, Vector3 velocity)
        {
            _position = position;
            _velocity = velocity;

            gameObject.SetActive(true);
        }

        // Call this method once per frame
        public void Move()
        {
            _position += _velocity * Time.deltaTime;
            transform.position = _position;
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}