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

        public void Create(Vector3 position, Vector3 velocity, Vector3 border, int lane, IDisposable disposable)
        {
            _lane = lane;
            Create(position, velocity, border, disposable);
        }
    }
}