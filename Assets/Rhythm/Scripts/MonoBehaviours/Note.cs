using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class Note : LaneObject, IJudgeable
    {
        protected double _justTime;

        protected double[] _judgeRange;
        protected IColorInput _colorInput;

        public void Initialize(NoteColor color, bool isLarge, double[] judgeRange, IColorInput colorInput)
        {
            _judgeRange = judgeRange;
           _colorInput = colorInput;
            Initialize(color, isLarge);
        }

        public void Create(Vector3 position, Vector3 velocity, int lane, double justTime)
        {
            _justTime = justTime;
            Create(position, velocity, lane);
        }

        public virtual Judgement Judge(double time)
        {
            return default;
        }
    }
}