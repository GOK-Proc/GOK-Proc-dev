using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class LaneObject : RhythmGameObject
    {
        protected int _lane;

        protected NoteColor _color;
        protected bool _isLarge;

        public void Initialize(NoteColor color, bool isLarge)
        {
            _color = color;
            _isLarge = isLarge;
        }

        public void Create(Vector3 position, Vector3 velocity, int lane)
        {
            _lane = lane;
            Create(position, velocity);
        }
    }
}