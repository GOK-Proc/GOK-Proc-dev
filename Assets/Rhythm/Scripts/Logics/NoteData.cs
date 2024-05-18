namespace Rhythm
{
    public struct NoteData
    {
        public float Speed;
        public int Lane;
        public NoteColor Color;
        public bool IsLarge;
        public double JustTime;
        public double Length;
        public double Bpm;

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
}