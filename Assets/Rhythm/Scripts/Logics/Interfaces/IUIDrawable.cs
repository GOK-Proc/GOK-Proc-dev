﻿namespace Rhythm
{
    public interface IUIDrawable
    {
        void DrawHeader(in HeaderInformation header, bool isTutorial);
        void DrawCombo(int combo);
        void DrawBattleResult(in HeaderInformation header, bool isWin, float playerHitPoint, float playerMaxHitPoint, float enemyHitPoint, float enemyMaxHitPoint, JudgeCount judgeCount, int maxCombo, bool isTutorial);
        void DrawRhythmResult(in HeaderInformation header, bool isClear, JudgeCount judgeCount, int maxCombo, int score, ScoreRank scoreRank, int highScore);
        void DrawScore(int score);
    }
}