using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IEffectDrawer
    {
        void DrawJudgeEffect(Vector3 position, Judgement judgement);
    }
}