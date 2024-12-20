﻿using System;
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

        public int Lane => _lane;
        public NoteColor Color => _color;
        public bool IsLarge => _isLarge;

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, IDisposable disposable)
        {
            Create(position, velocity, survivalRect, lane, null, disposable);
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, Action<Action> onCompleted, IDisposable disposable)
        {
            _lane = lane;
            Create(position, velocity, survivalRect, onCompleted, disposable);
        }
    }
}