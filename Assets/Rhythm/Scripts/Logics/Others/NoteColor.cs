namespace Rhythm
{
    public enum NoteColor
    {
        Undefined,
        Red,
        Blue,
    }

    public static class NoteColorExtension
    {
        public static string ToStringQuickly(this NoteColor color)
        {
            return color switch
            {
                NoteColor.Red => "Red",
                NoteColor.Blue => "Blue",
                _ => throw new System.InvalidOperationException()
            };
        }
    }
}