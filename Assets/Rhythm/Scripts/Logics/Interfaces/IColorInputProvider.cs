namespace Rhythm
{
    public interface IColorInputProvider
    {
        bool IsColorPressed(NoteColor color);
        bool IsColorPressedThisFrame(NoteColor color);
        bool IsColorJudged(NoteColor color);
        void CompleteColorJudge(NoteColor color);
        bool IsColorInputValid { get; set; }
    }
}