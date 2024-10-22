namespace Rhythm
{
    public interface IJudgeCountable
    {
        JudgeCount JudgeCount { get; }
        void CountUpJudgeCounter(Judgement judgement);
    }
}
