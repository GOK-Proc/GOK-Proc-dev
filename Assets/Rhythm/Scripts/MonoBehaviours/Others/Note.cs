using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable
    {
        protected double _justTime;

        protected JudgeRange _judgeRange;
        protected ITimeProvider _timeProvider;
        protected IColorInputProvider _colorInputProvider;
        protected IActiveLaneProvider _activeLaneProvider;

        public void Initialize(NoteColor color, bool isLarge, JudgeRange judgeRange, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _judgeRange = judgeRange;
            _timeProvider = timeProvider;
            _colorInputProvider = colorInputProvider;
            _activeLaneProvider = activeLaneProvider;
            Initialize(color, isLarge);
        }

        public virtual void Create(Vector3 position, Vector3 velocity, int lane, double justTime)
        {
            _justTime = justTime;
            Create(position, velocity, lane);
        }

        public virtual Judgement Judge()
        {
            return default;
        }
    }
}