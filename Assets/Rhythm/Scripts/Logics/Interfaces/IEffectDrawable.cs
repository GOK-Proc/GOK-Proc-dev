using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IEffectDrawable
    {
        void DrawJudgeEffect(Vector3 position, Judgement judgement);
        void DrawBattleEffect(Vector3 position, NoteColor color, bool isLarge, Judgement judgement);
    }
}