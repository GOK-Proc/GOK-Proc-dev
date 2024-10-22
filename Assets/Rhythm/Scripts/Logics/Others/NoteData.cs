namespace Rhythm
{
    public readonly struct NoteData
    {
        public readonly float Speed;
        public readonly int Lane;
        public readonly NoteColor Color;
        public readonly bool IsLarge;
        public readonly double JustTime;
        public readonly double Length;
        public readonly double Bpm;

        public NoteData(float speed, int lane, NoteColor color, bool isLarge, double justTime, double length, double bpm)
        {
            Speed = speed;
            Lane = lane;
            Color = color;
            IsLarge = isLarge;
            JustTime = justTime;
            Length = length;
            Bpm = bpm;
        }
    }

    public readonly struct LineData
    {
        public readonly float Speed;
        public readonly double JustTime;

        public LineData(float speed, double justTime)
        {
            Speed = speed;
            JustTime = justTime;
        }
    }
}