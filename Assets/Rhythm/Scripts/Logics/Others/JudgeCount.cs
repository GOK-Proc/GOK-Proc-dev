public readonly struct JudgeCount
{
    public readonly int Perfect;
    public readonly int Good;
    public readonly int False;

    public JudgeCount(int p, int g, int f)
    {
        Perfect = p;
        Good = g;
        False = f;
    }
}