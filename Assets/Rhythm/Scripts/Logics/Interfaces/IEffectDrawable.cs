using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IEffectDrawable
    {
        void DrawJudgeEffect(Vector3 position, Judgement judgement);
        void DrawJudgeFontEffect(Vector3 position, Judgement judgement);
        public void DrawLaneFlash(Vector3 position, NoteColor color);
        void DrawBattleEffect(Vector3 position, NoteColor color, bool isLarge, Judgement judgement, int id);
        void DrawEnemyAttackEffect(float delay, int id);
        double GetTimeToCreateEnemyAttackEffect(double justTime);
    }
}