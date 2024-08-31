using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Rhythm
{
    public class TapNote : Note
    {
        public override Judgement Judge()
        {
            var judge = Judgement.Undefined;

            foreach (var activeLane in _activeLaneProvider.ActiveLanes)
            {
                if (_lane == activeLane)
                {
                    if (_colorInputProvider.IsColorPressedThisFrame(_color) && !_colorInputProvider.IsColorJudged(_color))
                    {
                        var d = Math.Abs(_timeProvider.Time - (_justTime + _judgeOffset));

                        if (d <= _judgeRange.Perfect)
                        {
                            _colorInputProvider.CompleteColorJudge(_color);
                            judge = Judgement.Perfect;
                            break;
                        }
                        else if (d <= _judgeRange.Good)
                        {
                            _colorInputProvider.CompleteColorJudge(_color);
                            judge = Judgement.Good;
                            break;
                        }
                    }
                }
            }

            if (judge == Judgement.Undefined && _timeProvider.Time - (_justTime + _judgeOffset) > _judgeRange.Good)
            {
                judge = Judgement.False;
            }

            if (judge != Judgement.Undefined)
            {
                IsJudged = true;

                if (judge != Judgement.False)
                {
                    Destroy();
                }
            }

            return judge;
        }
    }
}