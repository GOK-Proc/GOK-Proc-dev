using DG.Tweening;
using Settings;

namespace Rhythm
{
    public interface ITutorialDrawable
    {
        Tweener DrawTutorial(int index, KeyConfigId keyConfig);
        Tweener EraseTutorial();
    }
}