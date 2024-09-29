using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace Rhythm
{
    public class ScoreManger : IJudgeCountable, IComboCountable, IResultProvider, IBattleMode, IRhythmMode
    {
        private readonly bool _isVs;
        private readonly Difficulty _difficulty;
        private readonly string _id;

        private readonly int[] _judgeCount;

        private readonly float _playerMaxHitPoint;
        private readonly float _enemyMaxHitPoint;
        private readonly float _playerBasicDamage;
        private readonly float _enemyBasicDamage;
        private readonly int _noteCount;
        private readonly int _largeRate;

        private readonly float _clearGaugePoint;
        private readonly float _incrementalGaugePoint;

        private readonly IList<JudgeRate> _judgeRates;
        private readonly IList<ComboBonusElement> _comboBonus;
        private readonly IList<float> _scoreRates;
        private readonly IList<int> _scoreRankBorders;
        private readonly GaugeRate _gaugeRate;

        private readonly ISoundPlayable _soundPlayable;
        private readonly IGaugeDrawable _gaugeDrawable;
        private readonly IUIDrawable _uiDrawable;
        private readonly IDataHandler<RecordData[]> _recordDataHandler;

        private float _playerHitPoint;
        private float _enemyHitPoint;

        private float _gaugePoint;
        private bool _wasAlerted;
        private bool _wasOverkilled;

        private readonly int _maxScore = 1000000;
        private readonly float _maxGaugePoint = 10000;
        private readonly float _alertRate = 0.2f;

        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }
        public bool IsWin => _isVs && Mathf.CeilToInt(_playerHitPoint) >= Mathf.CeilToInt(_enemyHitPoint);
        public bool IsOverkill => _isVs && _enemyHitPoint == 0;
        public bool IsKnockout => _isVs && _playerHitPoint == 0;
        public bool IsKnockoutAfterEffect { get; private set; }
        public bool IsClear => !_isVs && _gaugePoint >= _clearGaugePoint;
        public int Score { get
            {
                float s = 0;
                for (int i = 0; i < System.Enum.GetValues(typeof(Judgement)).Length - 1; i++)
                {
                    s += _scoreRates[i] * _judgeCount[i];
                }
                return (int)(s * _maxScore / _noteCount);
            } }

        public ScoreRank ScoreRank { get
            {
                int rankCount = System.Enum.GetValues(typeof(ScoreRank)).Length;
                for (int i = 0; i < rankCount - 1; i++)
                {
                    if (Score >= _scoreRankBorders[i]) return (ScoreRank)i;
                }
                return (ScoreRank)(rankCount - 1);
            } }

        public JudgeCount JudgeCount => new JudgeCount(_judgeCount[0], _judgeCount[1], _judgeCount[2]);

        public ScoreManger(bool isVs, string id, Difficulty difficulty, IList<JudgeRate> judgeRates, IList<LostRate> lostRates, IList<ComboBonus> comboBonus, IList<float> scoreRates, IList<int> scoreRankBorders, IList<GaugeRate> gaugeRates, int noteCount, (int attack, int defense) notePointCount, float playerHitPoint, int largeRate, ISoundPlayable soundPlayable, IGaugeDrawable gaugeDrawable, IUIDrawable uiDrawable, IDataHandler<RecordData[]> recordDataHandler)
        {
            _isVs = isVs;
            _difficulty = difficulty;
            _id = id;

            _judgeCount = new int[System.Enum.GetValues(typeof(Judgement)).Length - 1];
            _judgeRates = judgeRates;
            _comboBonus = comboBonus[(int)difficulty].Bonuses.OrderByDescending(x => x.Combo).ToArray();
            _scoreRates = scoreRates;
            _scoreRankBorders = scoreRankBorders;
            _gaugeRate = gaugeRates[(int)difficulty];

            _playerMaxHitPoint = playerHitPoint;
            _playerHitPoint = _playerMaxHitPoint;

            var victory = lostRates[(int)difficulty].Victory;
            var overkill = lostRates[(int)difficulty].Overkill;
            var knockout = lostRates[(int)difficulty].Knockout;

            _enemyMaxHitPoint = _playerMaxHitPoint * (knockout - victory) * (1 - overkill) / (knockout * (victory - overkill));
            _enemyHitPoint = _enemyMaxHitPoint;

            _playerBasicDamage = _playerMaxHitPoint / (notePointCount.defense * knockout);
            _enemyBasicDamage = _playerMaxHitPoint * (knockout - victory) / (notePointCount.attack * knockout * (victory - overkill));

            _gaugePoint = 0;
            _incrementalGaugePoint = _maxGaugePoint / (noteCount * _gaugeRate.PerfectRate);
            _clearGaugePoint = _maxGaugePoint * _gaugeRate.Border;

            _noteCount = noteCount;
            _largeRate = largeRate;

            _soundPlayable = soundPlayable;
            _gaugeDrawable = gaugeDrawable;
            _uiDrawable = uiDrawable;
            _recordDataHandler = recordDataHandler;

            _wasAlerted = false;
            _wasOverkilled = false;

            Combo = 0;
            MaxCombo = 0;
            IsKnockoutAfterEffect = false;

            _gaugeDrawable.DrawPlayerGauge(_playerHitPoint, _playerMaxHitPoint);
            _gaugeDrawable.DrawEnemyGauge(_enemyHitPoint, _enemyMaxHitPoint);
        }

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
                                var enemyDamage = i.Value;
                                _enemyHitPoint = CalculateHitPoint(_enemyHitPoint, _enemyMaxHitPoint, -enemyDamage);

                                var overkill = false;
                                if (IsOverkill)
                                {
                                    if (!_wasOverkilled)
                                    {
                                        overkill = true;
                                        _wasOverkilled = true;
                                    }
                                }

                                if (enemyDamage > 0)
                                {
                                    var hitPoint = _enemyHitPoint;
                                    var maxHitPoint = _enemyMaxHitPoint;
                                    _gaugeDrawable.DelayAttackDuration().OnComplete(() =>
                                    {
                                        _gaugeDrawable.DrawEnemyGauge(hitPoint, maxHitPoint);
                                        _gaugeDrawable.DrawEnemyDamageEffect();
                                        if (overkill) _soundPlayable.PlaySE("Overkill");
                                    });
                                }

                                break;

                            case BonusType.Heal:
                                var playerHealing = i.Value;
                                _playerHitPoint = CalculateHitPoint(_playerHitPoint, _playerMaxHitPoint, playerHealing);

                                if (playerHealing > 0)
                                {
                                    _gaugeDrawable.DrawPlayerGauge(_playerHitPoint, _playerMaxHitPoint);
                                    _gaugeDrawable.DrawPlayerHealEffect();
                                    _soundPlayable.PlaySE("PlayerHeal");
                                }
                                break;
                        }
                        break;
                    }
                }
            }

            switch (color)
            {
                case NoteColor.Red:
                    var enemyDamage = _enemyBasicDamage * (isLarge ? _largeRate : 1) * _judgeRates[(int)judgement - 1].Attack;
                    _enemyHitPoint = CalculateHitPoint(_enemyHitPoint, _enemyMaxHitPoint, -enemyDamage);

                    var overkill = false;
                    if (IsOverkill)
                    {
                        if (!_wasOverkilled)
                        {
                            overkill = true;
                            _wasOverkilled = true;
                        }
                    }

                    if (enemyDamage > 0)
                    {
                        var hitPoint = _enemyHitPoint;
                        var maxHitPoint = _enemyMaxHitPoint;
                        _gaugeDrawable.DelayAttackDuration().OnComplete(() =>
                        {
                            _gaugeDrawable.DrawEnemyGauge(hitPoint, maxHitPoint);
                            _gaugeDrawable.DrawEnemyDamageEffect();
                            if (overkill) _soundPlayable.PlaySE("Overkill");
                        });
                    }

                    break;

                case NoteColor.Blue:
                    var playerDamage = _playerBasicDamage * (isLarge ? _largeRate : 1) * _judgeRates[(int)judgement - 1].Defense;
                    _playerHitPoint = CalculateHitPoint(_playerHitPoint, _playerMaxHitPoint, -playerDamage);

                    var alert = false;
                    if (_playerHitPoint / _playerMaxHitPoint <= _alertRate)
                    {
                        if (!_wasAlerted)
                        {
                            alert = true;
                            _wasAlerted = true;
                        }
                    }
                    else
                    {
                        _wasAlerted = false;
                    }

                    if (playerDamage > 0)
                    {
                        var hitPoint = _playerHitPoint;
                        var maxHitPoint = _playerMaxHitPoint;
                        _gaugeDrawable.DelayDefenseDuration().OnComplete(() =>
                        {
                            _gaugeDrawable.DrawPlayerGauge(hitPoint, maxHitPoint);
                            _gaugeDrawable.DrawPlayerDamageEffect();
                            _soundPlayable.PlaySE("PlayerDamage");
                            IsKnockoutAfterEffect = hitPoint == 0;
                            if (alert) _soundPlayable.PlaySE("Alert");
                        });
                    }

                    break;
            }
        }

        public void Hit(Judgement judgement)
        {
            if (judgement == Judgement.Undefined) return;

            _uiDrawable.DrawScore(Score);
            _gaugePoint += _incrementalGaugePoint * judgement switch
            {
                Judgement.Perfect => 1,
                Judgement.Good => _gaugeRate.GoodCoefficient,
                Judgement.False => _gaugeRate.FalseCoefficient,
                _ => 0,
            };

            _gaugePoint = Mathf.Clamp(_gaugePoint, 0f, _maxGaugePoint);
            _gaugeDrawable.DrawClearGauge(_maxGaugePoint, _gaugePoint, _clearGaugePoint);
        }

        public void DisplayResult(in HeaderInformation header)
        {
            if (_isVs)
            {
                _uiDrawable.DrawBattleResult(header, IsWin, _playerHitPoint, _playerMaxHitPoint, _enemyHitPoint, _enemyMaxHitPoint, JudgeCount, MaxCombo);
            }
            else
            {
                _uiDrawable.DrawRhythmResult(header, IsClear, JudgeCount, MaxCombo, Score, ScoreRank, _recordDataHandler[_id][(int)_difficulty].Score);
            }
        }

        public void SaveRecordData()
        {
            if (!_isVs)
            {
                var records = _recordDataHandler[_id];
                var record = records[(int)_difficulty];
                var achievement = _judgeCount[2] == 0 ? (_judgeCount[1] == 0 ? Achievement.AllPerfect : Achievement.FullCombo) : Achievement.None;

                records[(int)_difficulty] = new RecordData(Mathf.Max(Score, record.Score), IsClear || record.IsCleared, Mathf.Max(MaxCombo, record.MaxCombo), (Achievement)Mathf.Max((int)achievement, (int)record.Achievement), Score > record.Score ? JudgeCount : record.JudgeCount);

                _recordDataHandler[_id] = records;
            }
        }
    }
}