using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rhythm
{
    public class HoldNote : Note
    {
        private Judgement _judgement;

        public override void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, double justTime, int id, IDisposable disposable)
        {
            _judgement = default;
            base.Create(position, velocity, survivalRect, lane, justTime, id, disposable);
        }

        public override Judgement Judge()
        {
            if (_colorInputProvider.IsColorPressed(_color))
            {
                var d = Math.Abs(_timeProvider.Time - (_justTime + _judgeOffset));

                if (d <= _judgeRange.Perfect)
                {
                    _judgement = Judgement.Perfect;
                }
                else if (d <= _judgeRange.Good)
                {
                    _judgement = Judgement.Good;
                }

                if (_timeProvider.Time - (_justTime + _judgeOffset) >= 0 && _judgement != default)
                {
                    IsJudged = true;
                    Destroy();
                    return _judgement;
                }
                    
            }
            else
            {
                if (_judgement != default)
                {
                    IsJudged = true;
                    Destroy();
                    return _judgement;
                }
            }

            if (_timeProvider.Time - (_justTime + _judgeOffset) > _judgeRange.Good)
            {
                IsJudged = true;

                if (_judgement != default)
                {
                    Destroy();
                    return _judgement;
                }

                return Judgement.False;
            }

            return default;
        }
    }
}