using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Rhythm
{
    public class LaneObject : RhythmGameObject
    {
        protected int _lane;

        [SerializeField] protected NoteColor _color;
        [SerializeField] protected bool _isLarge;

        public NoteColor Color => _color;
        public bool IsLarge => _isLarge;

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, IDisposable disposable)
        {
            _lane = lane;
            Create(position, velocity, survivalRect, disposable);
        }
    }
}