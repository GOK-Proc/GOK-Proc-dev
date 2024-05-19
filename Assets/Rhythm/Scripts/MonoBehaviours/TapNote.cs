using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rhythm
{
    public class TapNote : Note
    {
        public override Judgement Judge(double time, int currentLane)
        {
            if (_lane == currentLane)
            {
                if (_colorInput.IsColorPressedThisFrame(_color) && !_colorInput.GetColorPressedFlag(_color))
                {
                    var d = Math.Abs(time - _justTime);

                    if (d <= _judgeRange.Perfect)
                    {
                        _colorInput.SetColorPressedFlag(_color);
                        return Judgement.Perfect;
                    }
                    else if (d <= _judgeRange.Good)
                    {
                        _colorInput.SetColorPressedFlag(_color);
                        return Judgement.Good;
                    }
                }
            }

            if (time - _justTime > _judgeRange.Good) return Judgement.False;

            return default;
        }
    }
}