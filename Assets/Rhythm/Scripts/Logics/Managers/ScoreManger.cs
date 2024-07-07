using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class ScoreManger : IJudgeCountable
    {
        private readonly int[] _judgeCount;

        private readonly float _playerHitPointMax;
        private readonly float _enemyHitPointMax;
        private readonly float _playerBasicDamage;
        private readonly float _enemyBasicDamage;

        private float _playerHitPoint;
        private float _enemyHitPoint;

        public ScoreManger(Difficulty difficulty, IList<LostRate> lostRate, (int attack, int defense) noteCount, float playerHitPoint)
        {
            _judgeCount = new int[System.Enum.GetValues(typeof(Judgement)).Length];

            _playerHitPointMax = playerHitPoint;
            _playerHitPoint = _playerHitPointMax;

            var victory = lostRate[(int)difficulty - 1].Victory;
            var overkill = lostRate[(int)difficulty - 1].Overkill;
            var knockout = lostRate[(int)difficulty - 1].Knockout;

            _enemyHitPointMax = _playerHitPointMax * (knockout - victory) * (1 - overkill) / (knockout * (victory - overkill));
            _enemyHitPoint = _enemyHitPointMax;

            _playerBasicDamage = _playerHitPointMax / (noteCount.defense * knockout);
            _enemyBasicDamage = _playerHitPointMax * (knockout - victory) / (noteCount.attack * knockout * (victory - overkill));
        }

        public JudgeCount JudgeCount { get => new JudgeCount(_judgeCount[0], _judgeCount[1], _judgeCount[2]); }

        public void CountUpJudgeCounter(Judgement judgement)
        {
            _judgeCount[(int)judgement - 1]++;

        }
    }
}