using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class LaneObject : RhythmGameObject
    {
        protected int _lane;

        [SerializeField] protected NoteColor _color;
        [SerializeField] protected bool _isLarge;

        public void Create(Vector3 position, Vector3 velocity, int lane)
        {
            _lane = lane;
            Create(position, velocity);
        }
    }
}