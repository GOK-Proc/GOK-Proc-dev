using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable
    {
        protected double _justTime;

        protected JudgeRange _judgeRange;
        protected IColorInputProvider _colorInputProvider;
        protected IActiveLaneProvider _activeLaneProvider;

        public void Initialize(JudgeRange judgeRange, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, IActiveLaneProvider activeLaneProvider)
        {
            _judgeRange = judgeRange;
            _colorInputProvider = colorInputProvider;
            _activeLaneProvider = activeLaneProvider;
            Initialize(timeProvider);
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