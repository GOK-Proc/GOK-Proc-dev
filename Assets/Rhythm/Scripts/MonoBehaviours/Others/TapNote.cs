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
            foreach (var activeLane in _activeLaneProvider.ActiveLanes)
            {
                if (_lane == activeLane)
                {
                    if (_colorInputProvider.IsColorPressedThisFrame(_color) && !_colorInputProvider.IsColorJudged(_color))
                    {
                        var d = Math.Abs(_timeProvider.Time - _justTime);

                        if (d <= _judgeRange.Perfect)
                        {
                            _colorInputProvider.CompleteColorJudge(_color);
                            return Judgement.Perfect;
                        }
                        else if (d <= _judgeRange.Good)
                        {
                            _colorInputProvider.CompleteColorJudge(_color);
                            return Judgement.Good;
                        }
                    }
                }
            }

            if (_timeProvider.Time - _justTime > _judgeRange.Good) return Judgement.False;

            return default;
        }
    }
}