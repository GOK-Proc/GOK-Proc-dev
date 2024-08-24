using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IBattle
    {
        bool IsWin { get; }

        void Hit(NoteColor color, bool isLarge, Judgement judgement);
        void DisplayBattleResult(in HeaderInformation header);
    }
}