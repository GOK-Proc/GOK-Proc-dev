using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable, IComparable<Note>
    {
        public bool IsJudged { get; protected set; }

        public int Id { get; private set; }

        protected double _justTime;

        protected JudgeRange _judgeRange;
        protected double _judgeOffset;
        protected ITimeProvider _timeProvider;
        protected IColorInputProvider _colorInputProvider;
        protected IActiveLaneProvider _activeLaneProvider;

        public void Initialize(JudgeRange judgeRange, double judgeOffset, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _judgeRange = judgeRange;
            _judgeOffset = judgeOffset;
            _timeProvider = timeProvider;
            _colorInputProvider = colorInputProvider;
            _activeLaneProvider = activeLaneProvider;
        }

        public virtual void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, double justTime, int id, IDisposable disposable)
        {
            _justTime = justTime + _judgeOffset;
            Create(position, velocity, survivalRect, lane, disposable);

            IsJudged = false;
            Id = id;
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
    }
}