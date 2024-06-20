namespace Rhythm
{
    [System.Serializable]
    public struct JudgeRange
    {
        public double Perfect;
        public double Good;
        
        public JudgeRange(double perfect, double good)
        {
            Perfect = perfect;
            Good = good;
        }
    }
}