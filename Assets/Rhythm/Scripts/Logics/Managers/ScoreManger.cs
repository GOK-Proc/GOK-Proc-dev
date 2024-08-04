using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class ScoreManger : IJudgeCountable, IBattle
    {
        private readonly int[] _judgeCount;

        private readonly float _playerHitPointMax;
        private readonly float _enemyHitPointMax;
        private readonly float _playerBasicDamage;
        private readonly float _enemyBasicDamage;

        private readonly IList<JudgeRate> _judgeRates;

        private readonly IGaugeDrawable _gaugeDrawable;

        private float _playerHitPoint;
        private float _enemyHitPoint;

        public float PlayerHitPointMax => _playerHitPointMax;
        public float EnemyHitPointMax => _enemyHitPointMax;
        public float PlayerHitPoint => _playerHitPoint;
        public float EnemyHitPoint => _enemyHitPoint;

        public ScoreManger(Difficulty difficulty, IList<JudgeRate> judgeRates, IList<LostRate> lostRates, (int attack, int defense) noteCount, float playerHitPoint, IGaugeDrawable gaugeDrawable)
        {
            _judgeCount = new int[System.Enum.GetValues(typeof(Judgement)).Length];
            _judgeRates = judgeRates;

            _playerHitPointMax = playerHitPoint;
            _playerHitPoint = _playerHitPointMax;

            var victory = lostRates[(int)difficulty - 1].Victory;
            var overkill = lostRates[(int)difficulty - 1].Overkill;
            var knockout = lostRates[(int)difficulty - 1].Knockout;

            _enemyHitPointMax = _playerHitPointMax * (knockout - victory) * (1 - overkill) / (knockout * (victory - overkill));
            _enemyHitPoint = _enemyHitPointMax;

            _playerBasicDamage = _playerHitPointMax / (noteCount.defense * knockout);
            _enemyBasicDamage = _playerHitPointMax * (knockout - victory) / (noteCount.attack * knockout * (victory - overkill));

            _gaugeDrawable = gaugeDrawable;
        }

        public JudgeCount JudgeCount { get => new JudgeCount(_judgeCount[0], _judgeCount[1], _judgeCount[2]); }

        public void CountUpJudgeCounter(Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;
            _judgeCount[(int)judgement - 1]++;
        }

        public void Hit(NoteColor color, bool isLarge, Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;

            switch (color)
            {
                case NoteColor.Red:
                    _enemyHitPoint -= _enemyBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Attack;
                    if (_enemyHitPoint < 0) _enemyHitPoint = 0;
                    break;
                case NoteColor.Blue:
                    _playerHitPoint -= _playerBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Defense;
                    if (_playerHitPoint < 0) _playerHitPoint = 0;
                    break;
            }

            _gaugeDrawable.UpdateHitPointGauge(_playerHitPoint, _playerHitPointMax, _enemyHitPoint, _enemyHitPointMax);
        }
    }
}