using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class ScoreManger : IJudgeCountable, IComboCountable, IBattle
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

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }

        public ScoreManger(Difficulty difficulty, IList<JudgeRate> judgeRates, IList<LostRate> lostRates, (int attack, int defense) noteCount, float playerHitPoint, IGaugeDrawable gaugeDrawable)
        {
            _judgeCount = new int[System.Enum.GetValues(typeof(Judgement)).Length];
            _judgeRates = judgeRates;

            _playerHitPointMax = playerHitPoint;
            _playerHitPoint = _playerHitPointMax;

            var victory = lostRates[(int)difficulty].Victory;
            var overkill = lostRates[(int)difficulty].Overkill;
            var knockout = lostRates[(int)difficulty].Knockout;

            _enemyHitPointMax = _playerHitPointMax * (knockout - victory) * (1 - overkill) / (knockout * (victory - overkill));
            _enemyHitPoint = _enemyHitPointMax;

            _playerBasicDamage = _playerHitPointMax / (noteCount.defense * knockout);
            _enemyBasicDamage = _playerHitPointMax * (knockout - victory) / (noteCount.attack * knockout * (victory - overkill));

            _gaugeDrawable = gaugeDrawable;

            Combo = 0;
            MaxCombo = 0;
        }

        public JudgeCount JudgeCount { get => new JudgeCount(_judgeCount[0], _judgeCount[1], _judgeCount[2]); }

        public void CountUpJudgeCounter(Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;
            _judgeCount[(int)judgement - 1]++;

            if (judgement != Judgement.False)
            {
                Combo++;
                if (Combo > MaxCombo) MaxCombo = Combo;
            }
            else
            {
                Combo = 0;
            }
        }

        public void Hit(NoteColor color, bool isLarge, Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;
            float damage;

            switch (color)
            {
                case NoteColor.Red:
                    damage = _enemyBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Attack;
                    _enemyHitPoint -= damage;
                    if (_enemyHitPoint < 0) _enemyHitPoint = 0;
                    if (damage > 0) _gaugeDrawable.DamageEnemy(_enemyHitPoint, _enemyHitPointMax);
                    break;
                case NoteColor.Blue:
                    damage = _playerBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Defense;
                    _playerHitPoint -= damage;
                    if (_playerHitPoint < 0) _playerHitPoint = 0;
                    if (damage > 0) _gaugeDrawable.DamagePlayer(_playerHitPoint, _playerHitPointMax);
                    break;
            }
        }
    }
}