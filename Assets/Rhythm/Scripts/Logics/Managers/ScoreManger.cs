using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rhythm
{
    public class ScoreManger : IJudgeCountable, IComboCountable, IBattle
    {
        private readonly int[] _judgeCount;

        private readonly float _playerMaxHitPoint;
        private readonly float _enemyMaxHitPoint;
        private readonly float _playerBasicDamage;
        private readonly float _enemyBasicDamage;

        private readonly IList<JudgeRate> _judgeRates;
        private readonly IList<ComboBonusElement> _comboBonus;

        private readonly IGaugeDrawable _gaugeDrawable;
        private readonly IUIDrawable _uiDrawable;

        private float _playerHitPoint;
        private float _enemyHitPoint;

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }

        public ScoreManger(Difficulty difficulty, IList<JudgeRate> judgeRates, IList<LostRate> lostRates, IList<ComboBonus> comboBonus, (int attack, int defense) noteCount, float playerHitPoint, IGaugeDrawable gaugeDrawable, IUIDrawable uiDrawable)
        {
            _judgeCount = new int[System.Enum.GetValues(typeof(Judgement)).Length];
            _judgeRates = judgeRates;
            _comboBonus = comboBonus[(int)difficulty].Bonuses.OrderByDescending(x => x.Combo).ToArray();

            _playerMaxHitPoint = playerHitPoint;
            _playerHitPoint = _playerMaxHitPoint;

            var victory = lostRates[(int)difficulty].Victory;
            var overkill = lostRates[(int)difficulty].Overkill;
            var knockout = lostRates[(int)difficulty].Knockout;

            _enemyMaxHitPoint = _playerMaxHitPoint * (knockout - victory) * (1 - overkill) / (knockout * (victory - overkill));
            _enemyHitPoint = _enemyMaxHitPoint;

            _playerBasicDamage = _playerMaxHitPoint / (noteCount.defense * knockout);
            _enemyBasicDamage = _playerMaxHitPoint * (knockout - victory) / (noteCount.attack * knockout * (victory - overkill));

            _gaugeDrawable = gaugeDrawable;
            _uiDrawable = uiDrawable;

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

            _uiDrawable.DrawCombo(Combo);
        }

        public void Hit(NoteColor color, bool isLarge, Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;

            static float CalculateHitPoint(float hitPoint, float max, float delta)
            {
                var p = hitPoint + delta;
                return Mathf.Clamp(p, 0f, max);
            }

            if (Combo > 0)
            {
                foreach (var i in _comboBonus)
                {
                    if (Combo % i.Combo == 0)
                    {
                        switch (i.Type)
                        {
                            case BonusType.Attack:
                                _enemyHitPoint = CalculateHitPoint(_enemyHitPoint, _enemyMaxHitPoint, -i.Value);
                                break;
                            case BonusType.Heal:
                                _playerHitPoint = CalculateHitPoint(_playerHitPoint, _playerMaxHitPoint, i.Value);
                                break;
                        }
                    }
                }
            }

            switch (color)
            {
                case NoteColor.Red:
                    var enemyDamage = _enemyBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Attack;
                    _enemyHitPoint = CalculateHitPoint(_enemyHitPoint, _enemyMaxHitPoint, -enemyDamage);
                    if (enemyDamage > 0) _gaugeDrawable.DamageEnemy(_enemyHitPoint, _enemyMaxHitPoint);
                    break;
                case NoteColor.Blue:
                    var playerDamage = _playerBasicDamage * (isLarge ? 5 : 1) * _judgeRates[(int)judgement - 1].Defense;
                    _playerHitPoint = CalculateHitPoint(_playerHitPoint, _playerMaxHitPoint, -playerDamage);
                    if (playerDamage > 0) _gaugeDrawable.DamagePlayer(_playerHitPoint, _playerMaxHitPoint);
                    break;
            }

        }

    }
}