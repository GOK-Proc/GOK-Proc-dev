using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public interface IUIDrawable
    {
        void DrawHeader(in HeaderInformation header);
        void DrawCombo(int combo);
        void DrawBattleResult(in HeaderInformation header, bool isWin, float playerHitPoint, float playerMaxHitPoint, float enemyHitPoint, float enemyMaxHitPoint, JudgeCount judgeCount, int maxCombo);
    }
}