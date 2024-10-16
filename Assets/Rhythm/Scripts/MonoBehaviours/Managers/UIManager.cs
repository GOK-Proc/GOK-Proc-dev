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
    public class UIManager : MonoBehaviour, IGaugeDrawable, IEffectDrawable, IUIDrawable, IPauseScreenDrawable, ITutorialDrawable, IDamageDrawable, ISkipScreenDrawable
    {
        [SerializeField] private Vector2 _screenUpperLeft;
        [SerializeField] private Vector2 _screenLowerRight;

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
        [SerializeField] private float _knockoutFadeDuration;
        [SerializeField] private float _warningPeriod;
        [SerializeField] private float _warningMaxAlpha;
        [SerializeField] private Vector3 _healEffectRelativePosition;
        [SerializeField] private Vector3 _comboPosition;

        [SerializeField] private float _hitTimeRatio;
        [SerializeField] private float _shakeDuration;

        [Space(20)]
        [SerializeField] private Transform _battleUI;
        [SerializeField] private Transform _hitPointTextTransform;
        [SerializeField] private SpriteRenderer _backgroundRenderer;
        [SerializeField] private SpriteRenderer _playerRenderer;
        [SerializeField] private SpriteRenderer _enemyRenderer;

        [SerializeField] private SpriteRenderer _playerGaugeRenderer;
        [SerializeField] private TextMeshProUGUI _playerHitPointText;
        [SerializeField] private SpriteRenderer _enemyGaugeRenderer;
        [SerializeField] private Transform[] _enemyGauges;
        [SerializeField] private TextMeshProUGUI _enemyHitPointText;

        [System.Serializable]
        private struct EffectPrefab
        {
            public NoteColor Color;
            public bool IsLarge;
            public EffectObject Prefab;
        }

        [SerializeField] private EffectObject[] _judgeEffectPrefabs;
        [SerializeField] private EffectPrefab[] _battleEffectPrefabs;
        [SerializeField] private EffectObject[] _enemyAttackEffectPrefabs;
        [SerializeField] private EffectObject[] _judgeFontPrefabs;
        [SerializeField] private EffectObject[] _laneFlashPrefabs;
        [SerializeField] private EffectObject[] _swordEffectPrefabs;
        [SerializeField] private EffectObject[] _shieldEffectPrefabs;
        [SerializeField] private EffectObject[] _burstEffectPrefabs;
        [SerializeField] private EffectObject _healEffectPrefab;
        [SerializeField] private EffectObject _comboAttackEffectPrefab;
        [SerializeField] private Transform _effectParent;
        [SerializeField] private CanvasGroup _knockout;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _composerText;
        [SerializeField] private TextMeshProUGUI _difficultyText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _comboText;

        [SerializeField] private float _comboPopupScale;
        [SerializeField] private float _comboPopupDuration;

        [SerializeField] private Image _warningLayer;

        private Transform _player;
        private Transform _enemy;

        private Transform _playerGauge;
        private Transform _enemyGauge;

        private Vector3 _playerPosition;
        private Vector3 _enemyPosition;
        private Vector3 _playerGuardPosition;

        private Sequence _playerDamageTween;
        private Sequence _enemyDamageTween;

        private Color _playerColor;
        private Color _enemyColor;

        private Tweener _comboTween;

        private Sequence _warningTween;

        private ObjectPool<EffectObject>[] _judgeEffectPools;
        private Dictionary<(NoteColor, bool), ObjectPool<EffectObject>> _battleEffectPools;
        private ObjectPool<EffectObject>[] _enemyAttackEffectPools;
        private ObjectPool<EffectObject>[] _judgeFontPools;
        private ObjectPool<EffectObject>[] _laneFlashPools;
        private ObjectPool<EffectObject>[] _swordEffectPools;
        private ObjectPool<EffectObject>[] _shieldEffectPools;
        private ObjectPool<EffectObject>[] _burstEffectPools;
        private ObjectPool<EffectObject> _healEffectPool;
        private ObjectPool<EffectObject> _comboAttackEffectPool;

        private Dictionary<int, EffectObject> _enemyAttackEffects;

        [System.Serializable]
        private struct BattleDamageDifference<T>
        {
            public T Normal;
            public T Damaged;
            public T Pinch;
        }

        [SerializeField] private BattleDamageDifference<Color> _battleGaugeColor;
        [SerializeField] private BattleDamageDifference<Sprite> _playerSprites;

        private Vector3 _playerGaugePosition;
        private Vector3 _enemyGaugePosition;
        private Vector3 _playerGaugeScale;
        private Vector3 _enemyGaugeScale;

        [SerializeField] private Color _damageColor;

        [SerializeField] private Color[] _difficultyColor;
        [SerializeField] private Color _tutorialColor;

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
        [SerializeField] private TextMeshProUGUI _battleResultPlayerHitPointText;
        [SerializeField] private Image _battleResultEnemyGaugeImage;
        [SerializeField] private RectTransform[] _battleResultEnemyGauges;
        [SerializeField] private TextMeshProUGUI _battleResultEnemyHitPointText;

        [Space(20)]
        [SerializeField] private RectTransform _rhythmResultBox;
        [SerializeField] private RectTransform _rhythmResultContents;
        [SerializeField] private RectTransform[] _rhythmResultLabels;
        [SerializeField] private RectTransform _ryhthmResultNewRecord;
        [SerializeField] private TextMeshProUGUI _rhythmResultTitle;
        [SerializeField] private TextMeshProUGUI _rhythmResultComposer;
        [SerializeField] private TextMeshProUGUI _rhythmResultDifficulty;
        [SerializeField] private TextMeshProUGUI _rhythmResultLevel;
        [SerializeField] private TextMeshProUGUI[] _rhythmResultJudgeCount;
        [SerializeField] private TextMeshProUGUI _rhythmResultMaxComboCount;
        [SerializeField] private TextMeshProUGUI[] _rhythmResultComboLabel;
        [SerializeField] private TextMeshProUGUI _rhythmResultScoreNumber;
        [SerializeField] private TextMeshProUGUI _rhythmResultScoreRank;
        [SerializeField] private TextMeshProUGUI _rhythmResultHighScoreNumber;

        [Space(20)]
        [SerializeField] private RectTransform _pauseBox;
        [SerializeField] private TextMeshProUGUI _countDownNumber;

        [Space(20)]
        [SerializeField] private RectTransform _tutorialBox;
        [SerializeField] private RectTransform[] _tutorialContents;

        [System.Serializable]
        private struct TutorialKeyConfig
        {
            public KeyConfig KeyConfig;
            public RectTransform[] RectTransforms;
        }

        [SerializeField] private TutorialKeyConfig[] _tutorialKeyConfigs;

        [Space(20)]
        [SerializeField] private CanvasGroup _skipBox;

        private Dictionary<KeyConfig, RectTransform[]> _tutorialKeyConfigDictionary;

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
        private CanvasGroup _pauseBoxCanvasGroup;
        private CanvasGroup _tutorialBoxCanvasGroup;
        private CanvasGroup _skipBoxCanvasGroup;

        [Space(20)]
        [SerializeField] private float _resultBoxDuration;
        [SerializeField] private float _resultContentsDuration;
        [SerializeField] private float _pauseBoxDuration;
        [SerializeField] private float _tutorialBoxDuration;
        [SerializeField] private float _skipBoxDuration;


        private void Awake()
        {
            _player = _playerRenderer.transform;
            _enemy = _enemyRenderer.transform;
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
            _enemyAttackEffectPools = _enemyAttackEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _judgeFontPools = _judgeFontPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _laneFlashPools = _laneFlashPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _swordEffectPools = _swordEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _shieldEffectPools = _shieldEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _burstEffectPools = _burstEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray(); 
            _healEffectPool = new ObjectPool<EffectObject>(_healEffectPrefab, _effectParent);
            _comboAttackEffectPool = new ObjectPool<EffectObject>(_comboAttackEffectPrefab, _effectParent);

            _enemyAttackEffects = new Dictionary<int, EffectObject>();

            _playerDamageTween = null;
            _enemyDamageTween = null;

            _playerColor = _playerRenderer.color;
            _enemyColor = _enemyRenderer.color;

            _comboTween = null;
            _warningTween = null;

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
            _pauseBoxCanvasGroup = _pauseBox.GetComponent<CanvasGroup>();
            _tutorialBoxCanvasGroup = _tutorialBox.GetComponent<CanvasGroup>();
            _skipBoxCanvasGroup = _skipBox.GetComponent<CanvasGroup>();

            _tutorialKeyConfigDictionary = _tutorialKeyConfigs.ToDictionary(x => x.KeyConfig, x => x.RectTransforms);
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

        private void SetHitPointText(TextMeshProUGUI text, float hitPoint, float maxHitPoint)
        {
            text.SetText("{0} <size=-6>/{1}", Mathf.CeilToInt(hitPoint), Mathf.CeilToInt(maxHitPoint));
        }

        public void DrawPlayerGauge(float hitPoint, float maxHitPoint)
        {
            var value = hitPoint / maxHitPoint;

            DrawGauge(_playerGauge, _playerGaugePosition, _playerGaugeScale, value);
            SetBattleGaugeColor(_playerGaugeRenderer, value);
            SetHitPointText(_playerHitPointText, hitPoint, maxHitPoint);
        }

        public void DrawEnemyGauge(float hitPoint, float maxHitPoint)
        {
            var value = hitPoint / maxHitPoint;

            DrawGauge(_enemyGauge, _enemyGaugePosition, _enemyGaugeScale, value);
            SetBattleGaugeColor(_enemyGaugeRenderer, value);
            SetHitPointText(_enemyHitPointText, hitPoint, maxHitPoint);
        }

        public void DrawEnemyGauges(float hitPoint, float maxHitPoint, float maxGauge)
        {
            var value = hitPoint > 0 && hitPoint % maxGauge == 0f ? 1f : hitPoint % maxGauge / maxGauge;
            var gaugeCount = (int)(hitPoint / maxGauge) + (hitPoint > 0 && hitPoint % maxGauge == 0f ? 0 : 1);

            DrawGauge(_enemyGauge, _enemyGaugePosition, _enemyGaugeScale, value);

            for (int i = 0; i < _enemyGauges.Length; i++)
            {
                _enemyGauges[i].gameObject.SetActive(i < gaugeCount);
            }

            SetBattleGaugeColor(_enemyGaugeRenderer, hitPoint < maxGauge ? value : 1f);

            SetHitPointText(_enemyHitPointText, hitPoint, maxHitPoint);
        }

        public Sequence DelayAttackDuration() => DOTween.Sequence().AppendInterval(_attackEffectDuration);
        
        public void DrawPlayerDamageEffect()
        {
            if (_playerDamageTween != null)
            {
                _playerDamageTween.Kill();
                _player.position = _playerPosition;
                _playerRenderer.color = _playerColor;
            }

            _playerDamageTween = DOTween.Sequence()
                .Append(_player.DOShakePosition(_shakeDuration))
                .Join(_playerRenderer.DOColor(_damageColor, _shakeDuration / 2).OnComplete(() =>
                {
                    _playerRenderer.DOColor(_playerColor, _shakeDuration / 2);
                }));

            _playerDamageTween.Play();
        }

        public Sequence DelayDefenseDuration() => DOTween.Sequence().AppendInterval(_defenseEffectDuration);

        public void DrawEnemyDamageEffect()
        {
            if (_enemyDamageTween != null)
            {
                _enemyDamageTween.Kill();
                _enemy.position = _enemyPosition;
                _enemyRenderer.color = _enemyColor;
            }

            _enemyDamageTween = DOTween.Sequence()
                .Append(_enemy.DOShakePosition(_shakeDuration))
                .Join(_enemyRenderer.DOColor(_damageColor, _shakeDuration / 2).OnComplete(() =>
                {
                    _enemyRenderer.DOColor(_enemyColor, _shakeDuration / 2);
                }));

            _enemyDamageTween.Play();
        }

        public void DrawPlayerHealEffect()
        {
            IDisposable disposable = _healEffectPool.Create(out var obj, out var _);
            obj.Create(disposable);
            obj.PlayAnimation(_playerPosition + _healEffectRelativePosition);
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
                            sequence.Append(s.DOFade(0f, _judgeFontFadeDuration));
                            sequence.Play().OnComplete(() =>
                            {
                                s.color = color;
                                d?.Invoke();
                            }).SetLink(gameObject);
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
                    }).SetLink(gameObject);

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
                                        IDisposable disposable = _swordEffectPools[Convert.ToInt32(isLarge)].Create(out var obj, out var _);
                                        obj.Create(disposable);
                                        obj.PlayAnimation(_enemyPosition + (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f);
                                        d?.Invoke();
                                    }).SetLink(gameObject);
                                },
                                (t, s, d) =>
                                {
                                    var scale = t.localScale;
                                    var color = s.color;
                                    var sequence = DOTween.Sequence();
                                    sequence.Append(t.DOScale(_battleEffectFadeScale, _battleEffectFadeDuration));
                                    sequence.Join(s.DOFade(0f, _battleEffectFadeDuration));
                                    sequence.Play().OnComplete(() =>
                                    {
                                        t.localScale = scale;
                                        s.color = color;
                                        d?.Invoke();
                                    }).SetLink(gameObject);
                                }, 
                                disposable);

                            obj.PlayAnimation(position, _enemyPosition, isKeep: true);

                            break;
                        case NoteColor.Blue:

                            obj.Create(
                                (t, s, d) =>
                                {
                                    t.DOLocalMove(_playerGuardPosition, _defenseEffectDuration).OnComplete(() =>
                                    {
                                        IDisposable disposable = _shieldEffectPools[Convert.ToInt32(isLarge)].Create(out var obj, out var _);
                                        obj.Create(disposable);
                                        obj.PlayAnimation(_playerGuardPosition);
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
                                    }).SetLink(gameObject);
                                },
                                (t, s, d) =>
                                {
                                    var scale = t.localScale;
                                    var color = s.color;
                                    var sequence = DOTween.Sequence();
                                    sequence.Append(t.DOScale(_battleEffectFadeScale, _battleEffectFadeDuration));
                                    sequence.Join(s.DOFade(0f, _battleEffectFadeDuration));
                                    sequence.Play().OnComplete(() =>
                                    {
                                        t.localScale = scale;
                                        s.color = color;
                                        d?.Invoke();
                                    }).SetLink(gameObject);
                                },
                                disposable);

                            obj.PlayAnimation(position, _playerGuardPosition, isKeep: true);

                            break;
                    }
                    
                    break;
            }
        }

        public void DrawEnemyAttackEffect(bool isLarge, float delay, int id)
        {

            IDisposable disposable = _enemyAttackEffectPools[Convert.ToInt32(isLarge)].Create(out var obj, out var _);
            obj.Create((t, s, d) =>
            {
                t.DOLocalMove(_playerPosition, 1f).OnComplete(() =>
                {
                    IDisposable disposable = _burstEffectPools[Convert.ToInt32(isLarge)].Create(out var obj, out var _);
                    obj.Create(disposable);
                    obj.PlayAnimation(_playerPosition + (Vector3)UnityEngine.Random.insideUnitCircle * 0.5f);
                    d?.Invoke();
                }).SetEase(Ease.Linear).SetLink(gameObject);
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
                }).SetLink(gameObject);
            },
            disposable);

            _enemyAttackEffects.Add(id, obj);

            void Draw()
            {
                obj.PlayAnimation(_enemyPosition, true, true);
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

        public void DrawComboAttackEffect()
        {
            IDisposable disposable = _comboAttackEffectPool.Create(out var obj, out var _);

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
                    var scale = t.localScale;
                    var color = s.color;
                    var sequence = DOTween.Sequence();
                    sequence.Append(t.DOScale(_battleEffectFadeScale, _battleEffectFadeDuration));
                    sequence.Join(s.DOFade(0f, _battleEffectFadeDuration));
                    sequence.Play().OnComplete(() =>
                    {
                        t.localScale = scale;
                        s.color = color;
                        d?.Invoke();
                    }).SetLink(gameObject);
                },
            disposable);

            obj.PlayAnimation(_comboPosition, _enemyPosition, isKeep: true);
        }

        public void DrawHeader(in HeaderInformation header, bool isTutorial)
        {
            _titleText.SetText(header.Title);
            _composerText.SetText(header.Composer);

            if (isTutorial)
            {
                _difficultyText.SetText("TUTORIAL");
                _difficultyText.color = _tutorialColor;
            }
            else
            {
                _difficultyText.SetText(header.Difficulty switch
                {
                    Difficulty.Easy => "EASY",
                    Difficulty.Hard => "HARD",
                    Difficulty.Expert => "EXPERT",
                    _ => throw new InvalidEnumArgumentException()
                });
                _difficultyText.color = _difficultyColor[(int)header.Difficulty];
            }
            
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

        public void DrawBattleResult(in HeaderInformation header, bool isWin, float playerHitPoint, float playerMaxHitPoint, float enemyHitPoint, float enemyMaxHitPoint, JudgeCount judgeCount, int maxCombo, bool isTutorial)
        {
            _battleResultBoxCanvasGroup.alpha = 0f;
            _battleResultContentsCanvasGroup.alpha = 0f;
            _battleResultBox.gameObject.SetActive(true);
            _battleResultContents.gameObject.SetActive(true);

            _battleResultLabels[0].gameObject.SetActive(isWin);
            _battleResultLabels[1].gameObject.SetActive(!isWin);

            _battleResultTitle.SetText(header.Title);
            _battleResultComposer.SetText(header.Composer);

            if (isTutorial)
            {
                _battleResultDifficulty.SetText("TUTORIAL");
                _battleResultDifficulty.color = _tutorialColor;
            }
            else
            {
                _battleResultDifficulty.SetText(header.Difficulty switch
                {
                    Difficulty.Easy => "EASY",
                    Difficulty.Hard => "HARD",
                    Difficulty.Expert => "EXPERT",
                    _ => throw new InvalidEnumArgumentException()
                });
                _battleResultDifficulty.color = _difficultyColor[(int)header.Difficulty];
            }

            _battleResultLevel.SetText("Lv. {0}", header.Level);


            var playerValue = playerHitPoint / playerMaxHitPoint;
            DrawGauge(_battleResultPlayerGauge, _battleResultPlayerGaugePosition, _battleResultPlayerGaugeSizeDelta, playerValue);
            SetBattleGaugeColor(_battleResultPlayerGaugeImage, playerValue);
            SetHitPointText(_battleResultPlayerHitPointText, playerHitPoint, playerMaxHitPoint);

            var enemyValue = enemyHitPoint > 0 && enemyHitPoint % playerMaxHitPoint == 0f ? 1f : enemyHitPoint % playerMaxHitPoint / playerMaxHitPoint;
            var enemyGaugeCount = (int)(enemyHitPoint / playerMaxHitPoint) + (enemyHitPoint > 0 && enemyHitPoint % playerMaxHitPoint == 0f ? 0 : 1);
            DrawGauge(_battleResultEnemyGauge, _battleResultEnemyGaugePosition, _battleResultEnemyGaugeSizeDelta, enemyValue);

            for (int i = 0; i < _battleResultEnemyGauges.Length; i++)
            {
                _battleResultEnemyGauges[i].gameObject.SetActive(i < enemyGaugeCount);
            }

            SetBattleGaugeColor(_battleResultEnemyGaugeImage, enemyHitPoint < playerMaxHitPoint ? enemyValue : 1f);
            SetHitPointText(_battleResultEnemyHitPointText, enemyHitPoint, enemyMaxHitPoint);

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

        public void DrawRhythmResult(in HeaderInformation header, bool isClear, JudgeCount judgeCount, int maxCombo, int score, ScoreRank scoreRank, int highScore)
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

            _rhythmResultHighScoreNumber.SetText($"{highScore:N0}");

            _ryhthmResultNewRecord.gameObject.SetActive(score > highScore);

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
            _battleUI.gameObject.SetActive(isVs);
            _hitPointTextTransform.gameObject.SetActive(isVs);
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

        public Tweener DrawPauseScreen()
        {
            _pauseBoxCanvasGroup.alpha = 0f;
            _pauseBox.gameObject.SetActive(true);

            return _pauseBoxCanvasGroup.DOFade(1f, _pauseBoxDuration).SetUpdate(true);
        }

        public Tweener ErasePauseScreen() => _pauseBoxCanvasGroup.DOFade(0f, _pauseBoxDuration).SetUpdate(true).OnComplete(() =>
        {
            _pauseBox.gameObject.SetActive(false);
            _pauseBoxCanvasGroup.alpha = 1f;
        });

        public Sequence DrawCountDownScreen()
        {
            var sequence = DOTween.Sequence();

            var color = _countDownNumber.color;

            sequence.AppendInterval(0.5f).AppendCallback(() => { _countDownNumber.SetText("3"); _countDownNumber.gameObject.SetActive(true); })
                    .Append(_countDownNumber.DOFade(0f, 1f)).AppendCallback(() => { _countDownNumber.SetText("2"); _countDownNumber.color = color; })
                    .Append(_countDownNumber.DOFade(0f, 1f)).AppendCallback(() => { _countDownNumber.SetText("1"); _countDownNumber.color = color; })
                    .Append(_countDownNumber.DOFade(0f, 1f)).AppendCallback(() => { _countDownNumber.gameObject.SetActive(false); _countDownNumber.color = color; }).SetUpdate(true);

            return sequence;
        }

        public void DrawKnockout()
        {
            _knockout.alpha = 0f;
            _knockout.gameObject.SetActive(true);

            _knockout.DOFade(1f, _knockoutFadeDuration);
        }

        public Tweener DrawTutorial(int index, KeyConfig keyConfig)
        {
            if (index < _tutorialContents.Length)
            {
                for (int i = 0; i < _tutorialContents.Length; i++)
                {
                    _tutorialContents[i].gameObject.SetActive(i == index);
                }

                foreach (var i in _tutorialKeyConfigDictionary)
                {
                    i.Value[index].gameObject.SetActive(i.Key == keyConfig);
                }
                
                _tutorialBoxCanvasGroup.alpha = 0f;
                _tutorialBox.gameObject.SetActive(true);

                return _tutorialBoxCanvasGroup.DOFade(1f, _tutorialBoxDuration).SetUpdate(true);
            }

            return default;
        }

        public Tweener EraseTutorial() => _tutorialBoxCanvasGroup.DOFade(0f, _tutorialBoxDuration).SetUpdate(true).OnComplete(() =>
        {
            _tutorialBox.gameObject.SetActive(false);
            _tutorialBoxCanvasGroup.alpha = 1f;
        });

        public Tweener DrawSkipScreen()
        {
            _skipBoxCanvasGroup.alpha = 0f;
            _skipBox.gameObject.SetActive(true);

            return _skipBoxCanvasGroup.DOFade(1f, _skipBoxDuration).SetUpdate(true);
        }

        public Tweener EraseSkipScreen() => _skipBoxCanvasGroup.DOFade(0f, _skipBoxDuration).SetUpdate(true).OnComplete(() =>
        {
            _skipBox.gameObject.SetActive(false);
            _skipBoxCanvasGroup.alpha = 1f;
        });

        public void StartWarningLayer()
        {
            float alpha = _warningLayer.color.a;

            StopWarningLayer();

            _warningTween = DOTween.Sequence()
                .Append(_warningLayer.DOFade(_warningMaxAlpha, _warningPeriod / 2))
                .Append(_warningLayer.DOFade(alpha, _warningPeriod / 2))
                .SetLoops(-1);

            _warningLayer.gameObject.SetActive(true);
        }

        public void StopWarningLayer()
        {
            if (_warningTween != null)
            {
                _warningTween.Kill();
                _warningTween = null;
            }

            _warningLayer.gameObject.SetActive(false);
        }

        public void SetPlayerSprite(float hitPoint, float maxHitPoint)
        {
            var newSprite = (hitPoint / maxHitPoint) switch
            {
                <= 0.2f => _playerSprites.Pinch,
                <= 0.5f => _playerSprites.Damaged,
                _ => _playerSprites.Normal,
            };

            if (_playerRenderer.sprite != newSprite)
            {
                _playerRenderer.sprite = newSprite;
            }
        }

        public void DefeatPlayer()
        {
            if (_playerDamageTween != null)
            {
                _playerDamageTween.Kill();
                _player.position = _playerPosition;
            }

            _playerColor = new Color(0.5f, 0.5f, 0.5f);
            _playerRenderer.color = _playerColor;
        }

        public void DefeatEnemy()
        {
            if (_enemyDamageTween != null)
            {
                _enemyDamageTween.Kill();
                _enemy.position = _enemyPosition;
            }

            _enemyColor = new Color(0.5f, 0.5f, 0.5f);
            _enemyRenderer.color = _enemyColor;
        }

    }
}