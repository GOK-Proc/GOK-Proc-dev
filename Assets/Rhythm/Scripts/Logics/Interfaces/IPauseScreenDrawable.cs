using DG.Tweening;

namespace Rhythm
{
    public interface IPauseScreenDrawable
    {
        Tweener DrawPauseScreen();
        Tweener ErasePauseScreen();
        Sequence DrawCountDownScreen();
        void SetPauseCursorPositionY(float y);
    }
}