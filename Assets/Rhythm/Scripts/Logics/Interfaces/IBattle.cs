using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IBattle
    {
        void Hit(NoteColor color, bool isLarge, Judgement judgement);
    }
}