namespace Rhythm
{
    public interface IColorInput
    {
        bool IsColorPressed(NoteColor color);
        bool IsColorPressedThisFrame(NoteColor color);
    }
}