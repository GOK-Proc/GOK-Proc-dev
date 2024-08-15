using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IUIDrawable
    {
        void DrawHeader(string title, string composer, Difficulty difficulty, int level);
        void DrawCombo(int combo);
    }
}