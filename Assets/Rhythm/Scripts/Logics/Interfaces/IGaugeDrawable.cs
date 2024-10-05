using System;
using DG.Tweening;

namespace Rhythm
{
    public interface IGaugeDrawable
    {
        Sequence DelayAttackDuration();
        void DrawPlayerGauge(float hitPoint, float hitPointMax);
        void DrawPlayerDamageEffect();
        Sequence DelayDefenseDuration();
        void DrawEnemyGauge(float hitPoint, float hitPointMax);
        void DrawEnemyGauges(float hitPoint, float maxHitPoint, float maxGauge);
        void DrawEnemyDamageEffect();
        void DrawPlayerHealEffect();
        void DrawClearGauge(float maxGugePoint, float gaugePoint, float clearGaugePoint);
    }
}