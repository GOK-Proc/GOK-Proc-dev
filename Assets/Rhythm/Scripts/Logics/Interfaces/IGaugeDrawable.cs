using System;

namespace Rhythm
{
    public interface IGaugeDrawable
    {
        void DamagePlayer(float playerHitPoint, float playerHitPointMax, Action callback = null);
        void DamageEnemy(float enemyHitPoint, float enemyHitPointMax, Action callback = null);
        void HealPlayer(float hitPoint, float maxHitPoint, Action callback = null);
        void DrawClearGauge(float maxGugePoint, float gaugePoint, float clearGaugePoint);
    }
}