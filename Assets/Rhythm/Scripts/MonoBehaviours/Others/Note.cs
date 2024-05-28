using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable, IComparable<Note>
    {
        protected double _justTime;

        protected JudgeRange _judgeRange;
        protected ITimeProvider _timeProvider;
        protected IColorInputProvider _colorInputProvider;
        protected IActiveLaneProvider _activeLaneProvider;

        public void Initialize(JudgeRange judgeRange, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _judgeRange = judgeRange;
            _timeProvider = timeProvider;
            _colorInputProvider = colorInputProvider;
            _activeLaneProvider = activeLaneProvider;
        }

        public virtual void Create(Vector3 position, Vector3 velocity, Vector3 border, int lane, double justTime, IDisposable disposable)
        {
            _justTime = justTime;
            Create(position, velocity, border, lane, disposable);
        }

        public virtual Judgement Judge()
        {
            return default;
        }

        public int CompareTo(Note note)
        {
            if (note._justTime < _justTime)
            {
                return 1;
            }
            else if (note._justTime > _justTime)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public override string ToString()
        {
            return _justTime.ToString();
        }
    }
}