using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace Rhythm
{
    public class UIManager : MonoBehaviour, IGaugeDrawable, IEffectDrawable
    {
        [SerializeField] private Vector2 _screenUpperLeft;
        [SerializeField] private Vector2 _screenLowerRight;

        [SerializeField] private Transform _player;
        [SerializeField] private Transform _enemy;

        [SerializeField] private float _attackEffectDuration;
        [SerializeField] private float _defenseEffectDuration;
        [SerializeField] private float _enemyAttackEffectDuration;
        [SerializeField] private float _hitTimeRatio;
        [SerializeField] private float _shakeDuration;

        [Space(20)]
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
        [SerializeField] private Transform _effectParent;

        private Transform _playerGauge;
        private Transform _enemyGauge;

        private Vector3 _playerPosition;
        private Vector3 _enemyPosition;
        private Vector3 _playerGuardPosition;

        private Tweener _playerShakeTween;
        private Tweener _enemyShakeTween;

        private ObjectPool<EffectObject>[] _judgeEffectPools;
        private Dictionary<(NoteColor, bool), ObjectPool<EffectObject>> _battleEffectPools;
        private ObjectPool<EffectObject> _enemyAttackEffectPool;

        private Dictionary<int, EffectObject> _enemyAttackEffects;

        [System.Serializable]
        private struct GaugeColor
        {
            public Color Normal;
            public Color Damaged;
            public Color Pinch;
        }

        [SerializeField] private GaugeColor _gaugeColor;

        private Vector3 _playerGaugePosition;
        private Vector3 _enemyGaugePosition;
        private Vector3 _playerGaugeScale;
        private Vector3 _enemyGaugeScale;

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
            _enemyAttackEffects = new Dictionary<int, EffectObject>();

            _playerShakeTween = null;
            _enemyShakeTween = null;
        }

        public void DamagePlayer(float HitPoint, float MaxHitPoint)
        {
            void Draw()
            {
                var width = HitPoint * _playerGaugeScale.x / MaxHitPoint;
                var x = _playerGaugePosition.x - (_playerGaugeScale.x - width) / 2;

                _playerGauge.localPosition = new Vector3(x, _playerGaugePosition.y, _playerGaugePosition.z);
                _playerGauge.localScale = new Vector3(width, _playerGaugeScale.y, _playerGaugeScale.z);

                _playerGaugeRenderer.color = (HitPoint / MaxHitPoint) switch
                {
                    <= 0.1f => _gaugeColor.Pinch,
                    <= 0.5f => _gaugeColor.Damaged,
                    _ => _gaugeColor.Normal,
                };

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

        public void DamageEnemy(float HitPoint, float MaxHitPoint)
        {
            void Draw()
            {
                var width = HitPoint * _enemyGaugeScale.x / MaxHitPoint;
                var x = _enemyGaugePosition.x - (_enemyGaugeScale.x - width) / 2;

                _enemyGauge.localPosition = new Vector3(x, _enemyGaugePosition.y, _enemyGaugePosition.z);
                _enemyGauge.localScale = new Vector3(width, _enemyGaugeScale.y, _enemyGaugeScale.z);

                _enemyGaugeRenderer.color = (HitPoint / MaxHitPoint) switch
                {
                    <= 0.1f => _gaugeColor.Pinch,
                    <= 0.5f => _gaugeColor.Damaged,
                    _ => _gaugeColor.Normal,
                };

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
                                    sequence.Append(t.DOScale(1.5f, 0.3f));
                                    sequence.Join(s.DOFade(0f, 0.3f));
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
                                    sequence.Append(t.DOScale(1.5f, 0.3f));
                                    sequence.Join(s.DOFade(0f, 0.3f));
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
                sequence.Append(t.DOScale(1.5f, 0.3f));
                sequence.Join(s.DOFade(0f, 0.3f));
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
        
    }
}