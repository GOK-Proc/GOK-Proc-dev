using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class RhythmGameObject : MonoBehaviour, IMovable, IDestroyable
    {
        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }

        
        public void Create(Vector3 position, Vector3 velocity)
        {
            Position = position;
            Velocity = velocity;

            gameObject.SetActive(true);
        }

        public void Move()
        {
            
        }

        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}