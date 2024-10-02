using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface ITutorialDrawable
    {
        Tweener DrawTutorial(int index, KeyConfig keyConfig);
        Tweener EraseTutorial();
    }
}