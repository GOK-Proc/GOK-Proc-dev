using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Rhythm
{
    public interface IPauseScreenDrawable
    {
        Tweener DrawPauseScreen();
        Tweener ErasePauseScreen();
    }
}