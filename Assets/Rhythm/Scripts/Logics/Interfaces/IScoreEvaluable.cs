using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IScoreEvaluable
    {
        void Hit(NoteColor color, bool isLarge, Judgement judgement);
    }
}