using System;
using DG.Tweening;

namespace Rhythm
{
    public interface IGaugeDrawable
    {
        Sequence DelayAttackDuration();
        void DamagePlayer(float hitPoint, float hitPointMax);
        Sequence DelayDefenseDuration();
        void DamageEnemy(float hitPoint, float hitPointMax);
        void HealPlayer(float hitPoint, float maxHitPoint);
        void DrawClearGauge(float maxGugePoint, float gaugePoint, float clearGaugePoint);
    }
}