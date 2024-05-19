using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rhythm
{
    public class HoldNote : Note
    {
        private Judgement _judgement;

        public override void Create(Vector3 position, Vector3 velocity, int lane, double justTime)
        {
            _justTime = justTime;
            _judgement = default;
            Create(position, velocity, lane);
        }

        public override Judgement Judge(double time, int currentLane)
        {
            if (_colorInput.IsColorPressed(_color))
            {
                var d = Math.Abs(time - _justTime);

                if (d <= _judgeRange.Perfect)
                {
                    _judgement = Judgement.Perfect;
                }
                else if (d <= _judgeRange.Good)
                {
                    _judgement = Judgement.Good;
                }

                if (time - _justTime >= 0 && _judgement != default) return _judgement;
            }
            else
            {
                if (_judgement != default) return _judgement;
            }

            if (time - _justTime > _judgeRange.Good)
            {
                if (_judgement != default) return _judgement;
                return Judgement.False;
            }

            return default;
        }
    }
}