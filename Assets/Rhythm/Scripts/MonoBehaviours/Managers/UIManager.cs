using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace Rhythm
{
    public class UIManager : MonoBehaviour, IGaugeDrawable, IEffectDrawable, IUIDrawable
    {
        [SerializeField] private Vector2 _screenUpperLeft;
        [SerializeField] private Vector2 _screenLowerRight;

        [SerializeField] private Transform _player;
        [SerializeField] private Transform _enemy;

        [SerializeField] private float _attackEffectDuration;
        [SerializeField] private float _defenseEffectDuration;
        [SerializeField] private float _battleEffectFadeDuration;
        [SerializeField] private float _battleEffectFadeScale;
        [SerializeField] private float _enemyAttackEffectDuration;
        [SerializeField] private float _enemyAttackEffectFadeDuration;
        [SerializeField] private float _enemyAttackEffectFadeScale;
        [SerializeField] private float _judgeFontDuration;
        [SerializeField] private float _judgeFontFadeDuration;
        [SerializeField] private Vector3 _judgeFontDelta;
        [SerializeField] private float _laneFlashDuration;
        [SerializeField] private float _laneFlashFadeDuration;

        [SerializeField] private float _hitTimeRatio;
        [SerializeField] private float _shakeDuration;

        [Space(20)]
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private SpriteRenderer _enemyRenderer;

        [SerializeField] private SpriteRenderer _playerGaugeRenderer;
        [SerializeField] private SpriteRenderer _enemyGaugeRenderer;

        [System.Serializable]
        private struct EffectPrefab
        {
            public NoteColor Color;
            public bool IsLarge;
            public EffectObject Prefab;
        }

        [SerializeField] private EffectObject[] _judgeEffectPrefabs;
        [SerializeField] private EffectPrefab[] _battleEffectPrefabs;
        [SerializeField] private EffectObject _enemyAttackEffectPrefabs;
        [SerializeField] private EffectObject[] _judgeFontPrefabs;
        [SerializeField] private EffectObject[] _laneFlashPrefabs;
        [SerializeField] private Transform _effectParent;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _composerText;
        [SerializeField] private TextMeshProUGUI _difficultyText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _comboText;

        [SerializeField] private float _comboPopupScale;
        [SerializeField] private float _comboPopupDuration;


        private Transform _playerGauge;
        private Transform _enemyGauge;

        private Vector3 _playerPosition;
        private Vector3 _enemyPosition;
        private Vector3 _playerGuardPosition;

        private Tweener _playerShakeTween;
        private Tweener _enemyShakeTween;

        private Tweener _comboTween;

        private ObjectPool<EffectObject>[] _judgeEffectPools;
        private Dictionary<(NoteColor, bool), ObjectPool<EffectObject>> _battleEffectPools;
        private ObjectPool<EffectObject> _enemyAttackEffectPool;
        private ObjectPool<EffectObject>[] _judgeFontPools;
        private ObjectPool<EffectObject>[] _laneFlashPools;

        private Dictionary<int, EffectObject> _enemyAttackEffects;

        [System.Serializable]
        private struct BattleGaugeColor
        {
            public Color Normal;
            public Color Damaged;
            public Color Pinch;
        }

        [SerializeField] private BattleGaugeColor _battleGaugeColor;

        private Vector3 _playerGaugePosition;
        private Vector3 _enemyGaugePosition;
        private Vector3 _playerGaugeScale;
        private Vector3 _enemyGaugeScale;

        [SerializeField] private Color[] _difficultyColor;

        [Space(20)]
        [SerializeField] private RectTransform _score;
        [SerializeField] private TextMeshProUGUI _scoreNumber;
        [SerializeField] private RectTransform _clearGaugeLower;
        [SerializeField] private Image _clearGaugeUpperImage;

        private RectTransform _clearGaugeUpper;
        private Vector2 _clearGaugePosition;
        private Vector2 _clearGaugeSizeDelta;

        [System.Serializable]
        private struct ClearGaugeColor
        {
            public Color Normal;
            public Color Clear;
        }

        [SerializeField] private ClearGaugeColor _clearGaugeColor;

        [Space(20)]
        [SerializeField] private RectTransform _battleResultBox;
        [SerializeField] private RectTransform _battleResultContents;
        [SerializeField] private RectTransform[] _battleResultLabels;
        [SerializeField] private TextMeshProUGUI _battleResultTitle;
        [SerializeField] private TextMeshProUGUI _battleResultComposer;
        [SerializeField] private TextMeshProUGUI _battleResultDifficulty;
        [SerializeField] private TextMeshProUGUI _battleResultLevel;
        [SerializeField] private TextMeshProUGUI[] _battleResultJudgeCount;
        [SerializeField] private TextMeshProUGUI _battleResultMaxComboCount;
        [SerializeField] private TextMeshProUGUI[] _battleResultComboLabel;
        [SerializeField] private Image _battleResultPlayerGaugeImage;
        [SerializeField] private Image _battleResultEnemyGaugeImage;

        [Space(20)]
        [SerializeField] private RectTransform _rhythmResultBox;
        [SerializeField] private RectTransform _rhythmResultContents;
        [SerializeField] private RectTransform[] _rhythmResultLabels;
        [SerializeField] private TextMeshProUGUI _rhythmResultTitle;
        [SerializeField] private TextMeshProUGUI _rhythmResultComposer;
        [SerializeField] private TextMeshProUGUI _rhythmResultDifficulty;
        [SerializeField] private TextMeshProUGUI _rhythmResultLevel;
        [SerializeField] private TextMeshProUGUI[] _rhythmResultJudgeCount;
        [SerializeField] private TextMeshProUGUI _rhythmResultMaxComboCount;
        [SerializeField] private TextMeshProUGUI[] _rhythmResultComboLabel;
        [SerializeField] private TextMeshProUGUI _rhythmResultScoreNumber;
        [SerializeField] private TextMeshProUGUI _rhythmResultScoreRank;
        [SerializeField] private TextMeshProUGUI _rhythmResultRanking;

        private CanvasGroup _battleResultBoxCanvasGroup;
        private CanvasGroup _battleResultContentsCanvasGroup;
        private RectTransform _battleResultPlayerGauge;
        private RectTransform _battleResultEnemyGauge;
        private Vector2 _battleResultPlayerGaugePosition;
        private Vector2 _battleResultEnemyGaugePosition;
        private Vector2 _battleResultPlayerGaugeSizeDelta;
        private Vector2 _battleResultEnemyGaugeSizeDelta;

        private CanvasGroup _rhythmResultBoxCanvasGroup;
        private CanvasGroup _rhythmResultContentsCanvasGroup;

        [Space(20)]
        [SerializeField] private float _resultBoxDuration;
        [SerializeField] private float _resultContentsDuration;

        private void Awake()
        {
            _playerPosition = _player.position;
            _enemyPosition = _enemy.position;
            _playerGuardPosition = new Vector3(Mathf.Lerp(_enemyPosition.x, _playerPosition.x, _hitTimeRatio), _playerPosition.y, _playerPosition.z);

            _playerGauge = _playerGaugeRenderer.transform;
            _enemyGauge = _enemyGaugeRenderer.transform;

            _playerGaugePosition = _playerGauge.localPosition;
            _enemyGaugePosition = _enemyGauge.localPosition;
            _playerGaugeScale = _playerGauge.localScale;
            _enemyGaugeScale = _enemyGauge.localScale;

            _judgeEffectPools = _judgeEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _battleEffectPools = _battleEffectPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => new ObjectPool<EffectObject>(x.Prefab, _effectParent));
            _enemyAttackEffectPool = new ObjectPool<EffectObject>(_enemyAttackEffectPrefabs, _effectParent);
            _judgeFontPools = _judgeFontPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _laneFlashPools = _laneFlashPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();

            _enemyAttackEffects = new Dictionary<int, EffectObject>();

            _playerShakeTween = null;
            _enemyShakeTween = null;

            _clearGaugeUpper = _clearGaugeUpperImage.rectTransform;
            _clearGaugePosition = _clearGaugeLower.anchoredPosition;
            _clearGaugeSizeDelta = _clearGaugeLower.sizeDelta;

            _battleResultBoxCanvasGroup = _battleResultBox.GetComponent<CanvasGroup>();
            _battleResultContentsCanvasGroup = _battleResultContents.GetComponent<CanvasGroup>();

            _battleResultPlayerGauge = _battleResultPlayerGaugeImage.rectTransform;
            _battleResultEnemyGauge = _battleResultEnemyGaugeImage.rectTransform;
            _battleResultPlayerGaugePosition = _battleResultPlayerGauge.anchoredPosition;
            _battleResultEnemyGaugePosition = _battleResultEnemyGauge.anchoredPosition;
            _battleResultPlayerGaugeSizeDelta = _battleResultPlayerGauge.sizeDelta;
            _battleResultEnemyGaugeSizeDelta = _battleResultEnemyGauge.sizeDelta;

            _rhythmResultBoxCanvasGroup = _rhythmResultBox.GetComponent<CanvasGroup>();
            _rhythmResultContentsCanvasGroup = _rhythmResultContents.GetComponent<CanvasGroup>();
        }

        private void DrawGauge(Transform gauge, Vector3 position, Vector3 scale, float value)
        {
            var width = value * scale.x;
            var x = position.x - (scale.x - width) / 2;

            gauge.localPosition = new Vector3(x, position.y, position.z);
            gauge.localScale = new Vector3(width, scale.y, scale.z);
        }

        private void SetBattleGaugeColor(SpriteRenderer gaugeRenderer, float value)
        {
            gaugeRenderer.color = value switch
            {
                <= 0.2f => _battleGaugeColor.Pinch,
                <= 0.5f => _battleGaugeColor.Damaged,
                _ => _battleGaugeColor.Normal,
            };
        }

        private void DrawGauge(RectTransform gauge, Vector2 position, Vector2 sizeDelta, float value)
        {
            var width = value * sizeDelta.x;
            var x = position.x - (sizeDelta.x - width) / 2;

            gauge.anchoredPosition = new Vector2(x, position.y);
            gauge.sizeDelta = new Vector2(width, sizeDelta.y);
        }

        private void SetBattleGaugeColor(Image gaugeImage, float value)
        {
            gaugeImage.color = value switch
            {
                <= 0.2f => _battleGaugeColor.Pinch,
                <= 0.5f => _battleGaugeColor.Damaged,
                _ => _battleGaugeColor.Normal,
            };
        }

        private void SetClearGaugeColor(Image gaugeImage, float value, float border)
        {
            gaugeImage.color = value >= border ? _clearGaugeColor.Clear : _clearGaugeColor.Normal;
        }

        public void DamagePlayer(float hitPoint, float maxHitPoint)
        {
            void Draw()
            {
                var value = hitPoint / maxHitPoint;

                DrawGauge(_playerGauge, _playerGaugePosition, _playerGaugeScale, value);
                SetBattleGaugeColor(_playerGaugeRenderer, value);

                if (_playerShakeTween != null)
                {
                    _playerShakeTween.Kill();
                    _player.position = _playerPosition;
                }

                _playerShakeTween = _player.DOShakePosition(_shakeDuration);
            }

            IEnumerator DelayedDraw()
            {
                yield return new WaitForSeconds(_attackEffectDuration);

                Draw();
            }

            StartCoroutine(DelayedDraw());
        }

        public void DamageEnemy(float hitPoint, float maxHitPoint)
        {
            void Draw()
            {
                var value = hitPoint / maxHitPoint;

                DrawGauge(_enemyGauge, _enemyGaugePosition, _enemyGaugeScale, value);
                SetBattleGaugeColor(_enemyGaugeRenderer, value);

                if (_enemyShakeTween != null)
                {
                    _enemyShakeTween.Kill();
                    _enemy.position = _enemyPosition;
                }

                _enemyShakeTween = _enemy.DOShakePosition(_shakeDuration);
            }

            IEnumerator DelayedDraw()
            {
                yield return new WaitForSeconds(_defenseEffectDuration);

                Draw();
            }

            StartCoroutine(DelayedDraw());
        }

        public void HealPlayer(float hitPoint, float maxHitPoint)
        {
            var value = hitPoint / maxHitPoint;

            DrawGauge(_playerGauge, _playerGaugePosition, _playerGaugeScale, value);
            SetBattleGaugeColor(_playerGaugeRenderer, value);

            // ToDo: Effect

        }

        public void DrawJudgeEffect(Vector3 position, Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Perfect:
                case Judgement.Good:
                    IDisposable disposable = _judgeEffectPools[(int)judgement - 1].Create(out var obj, out var _);
                    obj.Create(disposable);
                    obj.PlayAnimation(position);
                    break;
            }
        }

        public void DrawJudgeFontEffect(Vector3 position, Judgement judgement)
        {
            switch (judgement)
            {
                case Judgement.Perfect:
                case Judgement.Good:
                case Judgement.False:
                    IDisposable disposable = _judgeFontPools[(int)judgement - 1].Create(out var obj, out var _);
                    obj.Create(
                        (t, s, d) =>
                        {
                            var color = s.color;

                            var sequence = DOTween.Sequence();
                            sequence.Append(t.DOLocalMove(position + _judgeFontDelta, _judgeFontDuration));
                            sequence.Join(s.DOFade(0f, _judgeFontFadeDuration).SetDelay(_judgeFontDuration - _judgeFontFadeDuration));
                            sequence.Play().OnComplete(() =>
                            {
                                s.color = color;
                                d?.Invoke();
                            });
                        },
                        (t, s, d) =>
                        {
                            d?.Invoke();
                        },
                        disposable
                        );

                    obj.Play(position);
                    break;
            }
        }

        public void DrawLaneFlash(Vector3 position, NoteColor color)
        {
            if (color == NoteColor.Undefined) return;

            IDisposable disposable = _laneFlashPools[(int)color - 1].Create(out var obj, out var _);
            obj.Create(
                (t, s, d) =>
                {
                    var scaleX = t.localScale.x;
                    t.localScale = new Vector3(0f, t.localScale.y, t.localScale.z);

                    t.DOScaleX(scaleX, _laneFlashDuration).OnComplete(() =>
                    {
                        d?.Invoke();
                    });
                },
                (t, s, d) =>
                {
                    var color = s.color;

                    s.DOFade(0f, _laneFlashFadeDuration).OnComplete(() =>
                    {
                        s.color = color;
                        d?.Invoke();
                    });

                },
                disposable);

            obj.Play(position);
        }

        public void DrawBattleEffect(Vector3 position, NoteColor color, bool isLarge, Judgement judgement, int id)
        {
            switch (judgement)
            {
                case Judgement.Perfect:
                case Judgement.Good:
                    IDisposable disposable = _battleEffectPools[(color, isLarge)].Create(out var obj, out var _);

                    switch (color)
                    {
                        case NoteColor.Red:

                            obj.Create(
                                (t, s, d) =>
                                {
                                    t.DOLocalMove(_enemyPosition, _attackEffectDuration).OnComplete(() =>
                                    {
                                        d?.Invoke();
                                    });
                                },
                                (t, s, d) =>
                                {
                                    var color = s.color;
                                    var sequence = DOTween.Sequence();
                                    sequence.Append(t.DOScale(_battleEffectFadeScale, _battleEffectFadeDuration));
                                    sequence.Join(s.DOFade(0f, _battleEffectFadeDuration));
                                    sequence.Play().OnComplete(() =>
                                    {
                                        t.localScale = Vector3.one;
                                        s.color = color;
                                        d?.Invoke();
                                    });
                                }, 
                                disposable);

                            obj.Play(position);

                            break;
                        case NoteColor.Blue:

                            obj.Create(
                                (t, s, d) =>
                                {
                                    t.DOLocalMove(_playerGuardPosition, _defenseEffectDuration).OnComplete(() =>
                                    {
                                        if (_enemyAttackEffects.TryGetValue(id, out var attack))
                                        {
                                            attack.Stop((t, s, d) =>
                                            {
                                                t.DOKill();
                                                s.DOKill();
                                                d?.Invoke();
                                            });
                                        }
                                        d?.Invoke();
                                    });
                                },
                                (t, s, d) =>
                                {
                                    var color = s.color;
                                    var sequence = DOTween.Sequence();
                                    sequence.Append(t.DOScale(_battleEffectFadeScale, _battleEffectFadeDuration));
                                    sequence.Join(s.DOFade(0f, _battleEffectFadeDuration));
                                    sequence.Play().OnComplete(() =>
                                    {
                                        t.localScale = Vector3.one;
                                        s.color = color;
                                        d?.Invoke();
                                    });
                                },
                                disposable);

                            obj.Play(position);

                            break;
                    }
                    
                    break;
            }
        }

        public void DrawEnemyAttackEffect(float delay, int id)
        {

            IDisposable disposable = _enemyAttackEffectPool.Create(out var obj, out var _);
            obj.Create((t, s, d) =>
            {
                t.DOLocalMove(_playerPosition, 1f).OnComplete(() =>
                {
                    d?.Invoke();
                }).SetEase(Ease.Linear);
            },
            (t, s, d) =>
            {
                var color = s.color;
                var sequence = DOTween.Sequence();
                sequence.Append(t.DOScale(_enemyAttackEffectFadeScale, _enemyAttackEffectFadeDuration));
                sequence.Join(s.DOFade(0f, _enemyAttackEffectFadeDuration));
                sequence.Play().OnComplete(() =>
                {
                    t.localScale = Vector3.one;
                    s.color = color;
                    d?.Invoke();
                });
            },
            disposable);

            _enemyAttackEffects.Add(id, obj);

            void Draw()
            {
                obj.Play(_enemyPosition);
            }

            if (delay > 0)
            {
                IEnumerator DelayedDraw()
                {
                    yield return new WaitForSeconds(delay);

                    Draw();
                }

                StartCoroutine(DelayedDraw());
            }
            else
            {
                Draw();
            }
        }

        public double GetTimeToCreateEnemyAttackEffect(double justTime) => justTime + _defenseEffectDuration - _hitTimeRatio * _enemyAttackEffectDuration;

        public void DrawHeader(in HeaderInformation header)
        {
            _titleText.SetText(header.Title);
            _composerText.SetText(header.Composer);
            _difficultyText.SetText(header.Difficulty switch {
                Difficulty.Easy => "EASY",
                Difficulty.Hard => "HARD",
                Difficulty.Expert => "EXPERT",
                _ => throw new InvalidEnumArgumentException()
            });
            _difficultyText.color = _difficultyColor[(int)header.Difficulty];
            _levelText.SetText("Lv. {0}", header.Level);
        }

        public void DrawCombo(int combo)
        {
            if (combo >= 5)
            {
                _comboText.SetText("{0} Combo!!", combo);
                _comboText.gameObject.SetActive(true);

                if (_comboTween != null)
                {
                    _comboTween.Kill();
                    _comboText.rectTransform.localScale = Vector3.one;
                }
                _comboTween = _comboText.rectTransform.DOScale(new Vector3(_comboPopupScale, _comboPopupScale, 1f), _comboPopupDuration / 2).OnComplete(() =>
                {
                    _comboText.rectTransform.DOScale(Vector3.one, _comboPopupDuration / 2);
                });
            }
            else
            {
                _comboText.gameObject.SetActive(false);
            }
        }

        public void DrawBattleResult(in HeaderInformation header, bool isWin, float playerHitPoint, float playerMaxHitPoint, float enemyHitPoint, float enemyMaxHitPoint, JudgeCount judgeCount, int maxCombo)
        {
            _battleResultBoxCanvasGroup.alpha = 0f;
            _battleResultContentsCanvasGroup.alpha = 0f;
            _battleResultBox.gameObject.SetActive(true);
            _battleResultContents.gameObject.SetActive(true);

            _battleResultLabels[0].gameObject.SetActive(isWin);
            _battleResultLabels[1].gameObject.SetActive(!isWin);

            _battleResultTitle.SetText(header.Title);
            _battleResultComposer.SetText(header.Composer);
            _battleResultDifficulty.SetText(header.Difficulty switch
            {
                Difficulty.Easy => "EASY",
                Difficulty.Hard => "HARD",
                Difficulty.Expert => "EXPERT",
                _ => throw new InvalidEnumArgumentException()
            });
            _battleResultDifficulty.color = _difficultyColor[(int)header.Difficulty];
            _battleResultLevel.SetText("Lv. {0}", header.Level);


            var playerValue = playerHitPoint / playerMaxHitPoint;
            DrawGauge(_battleResultPlayerGauge, _battleResultPlayerGaugePosition, _battleResultPlayerGaugeSizeDelta, playerValue);
            SetBattleGaugeColor(_battleResultPlayerGaugeImage, playerValue);

            var enemyValue = enemyHitPoint / enemyMaxHitPoint;
            DrawGauge(_battleResultEnemyGauge, _battleResultEnemyGaugePosition, _battleResultEnemyGaugeSizeDelta, enemyValue);
            SetBattleGaugeColor(_battleResultEnemyGaugeImage, enemyValue);

            var judges = new int[] { judgeCount.Perfect, judgeCount.Good, judgeCount.False };

            for (int i = 0; i < 3; i++)
            {
                _battleResultJudgeCount[i].SetText("{0}", judges[i]);
            }

            _battleResultMaxComboCount.SetText("{0}", maxCombo);

            _battleResultComboLabel[0].gameObject.SetActive(judgeCount.Good == 0 && judgeCount.False == 0);
            _battleResultComboLabel[1].gameObject.SetActive(judgeCount.Good > 0 && judgeCount.False == 0);

            var size = _battleResultBox.sizeDelta;
            _battleResultBox.sizeDelta = new Vector2(size.x, 0f);

            var sequence = DOTween.Sequence();

            sequence.Append(_battleResultBoxCanvasGroup.DOFade(1f, _resultBoxDuration));
            sequence.Join(_battleResultBox.DOSizeDelta(size, _resultBoxDuration));
            sequence.Append(_battleResultContentsCanvasGroup.DOFade(1f, _resultContentsDuration));

            sequence.Play();
        }

        public void DrawRhythmResult(in HeaderInformation header, bool isClear, JudgeCount judgeCount, int maxCombo, int score, ScoreRank scoreRank, int ranking)
        {
            _rhythmResultBoxCanvasGroup.alpha = 0f;
            _rhythmResultContentsCanvasGroup.alpha = 0f;
            _rhythmResultBox.gameObject.SetActive(true);
            _rhythmResultContents.gameObject.SetActive(true);

            _rhythmResultLabels[0].gameObject.SetActive(isClear);
            _rhythmResultLabels[1].gameObject.SetActive(!isClear);

            _rhythmResultTitle.SetText(header.Title);
            _rhythmResultComposer.SetText(header.Composer);
            _rhythmResultDifficulty.SetText(header.Difficulty switch
            {
                Difficulty.Easy => "EASY",
                Difficulty.Hard => "HARD",
                Difficulty.Expert => "EXPERT",
                _ => throw new InvalidEnumArgumentException()
            });
            _rhythmResultDifficulty.color = _difficultyColor[(int)header.Difficulty];
            _rhythmResultLevel.SetText("Lv. {0}", header.Level);

            _rhythmResultScoreNumber.SetText($"{score:N0}");
            _rhythmResultScoreRank.SetText(scoreRank switch
            {
                ScoreRank.SS => "SS",
                ScoreRank.S => "S",
                ScoreRank.A => "A",
                ScoreRank.B => "B",
                _ => "C"
            });

            _rhythmResultRanking.text = "Rank " + ranking + ranking switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };

            var judges = new int[] { judgeCount.Perfect, judgeCount.Good, judgeCount.False };

            for (int i = 0; i < 3; i++)
            {
                _rhythmResultJudgeCount[i].SetText("{0}", judges[i]);
            }

            _rhythmResultMaxComboCount.SetText("{0}", maxCombo);

            _rhythmResultComboLabel[0].gameObject.SetActive(judgeCount.Good == 0 && judgeCount.False == 0);
            _rhythmResultComboLabel[1].gameObject.SetActive(judgeCount.Good > 0 && judgeCount.False == 0);

            var size = _rhythmResultBox.sizeDelta;
            _rhythmResultBox.sizeDelta = new Vector2(size.x, 0f);

            var sequence = DOTween.Sequence();

            sequence.Append(_rhythmResultBoxCanvasGroup.DOFade(1f, _resultBoxDuration));
            sequence.Join(_rhythmResultBox.DOSizeDelta(size, _resultBoxDuration));
            sequence.Append(_rhythmResultContentsCanvasGroup.DOFade(1f, _resultContentsDuration));

            sequence.Play();
        }

        public void DrawScore(int score)
        {
            _scoreNumber.SetText($"{score:N0}");
        }

        public void SwitchUI(bool isVs)
        {
            _player.gameObject.SetActive(isVs);
            _enemy.gameObject.SetActive(isVs);
            _score.gameObject.SetActive(!isVs);
        }

        public void SetClearGaugeBorder(float border)
        {
            var position = _clearGaugeLower.anchoredPosition;
            var sizeDelta = _clearGaugeLower.sizeDelta;

            _clearGaugeLower.sizeDelta *= new Vector2(1 - border, 1f);

            var x = position.x + sizeDelta.x / 2 - _clearGaugeLower.sizeDelta.x / 2;
            _clearGaugeLower.anchoredPosition = new Vector2(x, _clearGaugeLower.anchoredPosition.y);
        }

        public void DrawClearGauge(float maxGugePoint, float gaugePoint, float clearGaugePoint)
        {
            var value = gaugePoint / maxGugePoint;
            var border = clearGaugePoint / maxGugePoint;

            DrawGauge(_clearGaugeUpper, _clearGaugePosition, _clearGaugeSizeDelta, value);
            SetClearGaugeColor(_clearGaugeUpperImage, value, border);
        }

        public void SetBackgroundSprite(Sprite sprite)
        {
            _backgroundRenderer.sprite = sprite;
        }

        public void SetEnemySprite(Sprite sprite)
        {
            _enemyRenderer.sprite = sprite;
        }

    }
}