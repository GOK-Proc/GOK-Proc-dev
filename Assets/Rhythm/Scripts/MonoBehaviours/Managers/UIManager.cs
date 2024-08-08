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

        [SerializeField] private Vector3 _playerPosition;
        [SerializeField] private Vector3 _enemyPosition;
        [SerializeField] private float _battleEffectDuration;

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
        [SerializeField] private Transform _effectParent;

        private Transform _playerGauge;
        private Transform _enemyGauge;

        private ObjectPool<EffectObject>[] _judgeEffectPools;
        private Dictionary<(NoteColor, bool), ObjectPool<EffectObject>> _battleEffectPools;

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
            _playerGauge = _playerGaugeRenderer.transform;
            _enemyGauge = _enemyGaugeRenderer.transform;

            _playerGaugePosition = _playerGauge.position;
            _enemyGaugePosition = _enemyGauge.position;
            _playerGaugeScale = _playerGauge.localScale;
            _enemyGaugeScale = _enemyGauge.localScale;

            _judgeEffectPools = _judgeEffectPrefabs.Select(x => new ObjectPool<EffectObject>(x, _effectParent)).ToArray();
            _battleEffectPools = _battleEffectPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => new ObjectPool<EffectObject>(x.Prefab, _effectParent));
        }

        public void UpdateHitPointGauge(float playerHitPoint, float playerHitPointMax, float enemyHitPoint, float enemyHitPointMax)
        {
            if (playerHitPointMax > 0 && enemyHitPointMax > 0)
            {
                var width = playerHitPoint * _playerGaugeScale.x / playerHitPointMax;
                var x = _playerGaugePosition.x - (_playerGaugeScale.x - width) / 2;

                _playerGauge.position = new Vector3(x, _playerGaugePosition.y, _playerGaugePosition.z);
                _playerGauge.localScale = new Vector3(width, _playerGaugeScale.y, _playerGaugeScale.z);

                width = enemyHitPoint * _enemyGaugeScale.x / enemyHitPointMax;
                x = _enemyGaugePosition.x - (_enemyGaugeScale.x - width) / 2;

                _enemyGauge.position = new Vector3(x, _enemyGaugePosition.y, _enemyGaugePosition.z);
                _enemyGauge.localScale = new Vector3(width, _enemyGaugeScale.y, _enemyGaugeScale.z);

                _playerGaugeRenderer.color = (playerHitPoint / playerHitPointMax) switch
                {
                    <= 0.1f => _gaugeColor.Pinch,
                    <= 0.5f => _gaugeColor.Damaged,
                    _ => _gaugeColor.Normal,
                };

                _enemyGaugeRenderer.color = (enemyHitPoint / enemyHitPointMax) switch
                {
                    <= 0.1f => _gaugeColor.Pinch,
                    <= 0.5f => _gaugeColor.Damaged,
                    _ => _gaugeColor.Normal,
                };
            }
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

        public void DrawBattleEffect(Vector3 position, NoteColor color, bool isLarge, Judgement judgement)
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
                                    t.DOLocalMove(_enemyPosition, _battleEffectDuration).OnComplete(() =>
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
                                    t.DOLocalMove(_playerPosition, _battleEffectDuration).OnComplete(() =>
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
                    }
                    
                    break;
            }
        }
    }
}