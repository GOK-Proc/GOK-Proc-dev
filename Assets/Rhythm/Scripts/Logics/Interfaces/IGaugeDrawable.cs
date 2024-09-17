namespace Rhythm
{
    public interface IGaugeDrawable
    {
        void DamagePlayer(float playerHitPoint, float playerHitPointMax);
        void DamageEnemy(float enemyHitPoint, float enemyHitPointMax);
        void HealPlayer(float hitPoint, float maxHitPoint);
        void DrawClearGauge(float maxGugePoint, float gaugePoint, float clearGaugePoint);
    }
}