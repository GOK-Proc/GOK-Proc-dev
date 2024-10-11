namespace Rhythm
{
    public interface IDamageDrawable
    {
        void StartWarningLayer();
        void StopWarningLayer();
        void SetPlayerSprite(float hitPoint, float maxHitPoint);
    }
}