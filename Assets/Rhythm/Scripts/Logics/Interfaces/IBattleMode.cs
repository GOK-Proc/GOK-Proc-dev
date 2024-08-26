using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IBattleMode
    {
        bool IsWin { get; }
        void Hit(NoteColor color, bool isLarge, Judgement judgement);
    }
}