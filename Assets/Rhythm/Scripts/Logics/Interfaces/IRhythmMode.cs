namespace Rhythm
{
    public interface IRhythmMode
    {
        bool IsClear { get; }
        int Score { get; }
        void Hit(Judgement judgement);
    }
}