using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class UIManager : MonoBehaviour, IGaugeDrawable, IEffectDrawable
    {
        [SerializeField] private Vector2 _screenUpperLeft;
        [SerializeField] private Vector2 _screenLowerRight;

        [Space(20)]
        [SerializeField] private SpriteRenderer _playerGaugeRenderer;
        [SerializeField] private SpriteRenderer _enemyGaugeRenderer;

        [System.Serializable]
        private struct EffectPrefab<T> where T : FrameEffect
        {
            public NoteColor Color;
            public bool IsLarge;
            public T Prefab;
        }

        [SerializeField] private FrameEffect[] _judgeEffectPrefabs;
        [SerializeField] private EffectPrefab<FrameEffect>[] _battleEffectPrefabs;
        [SerializeField] private Transform _effectParent;

        private Transform _playerGauge;
        private Transform _enemyGauge;

        private ObjectPool<FrameEffect>[] _judgeEffectPools;
        private Dictionary<(NoteColor, bool), ObjectPool<FrameEffect>> _battleEffectPools;

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

            _judgeEffectPools = _judgeEffectPrefabs.Select(x => new ObjectPool<FrameEffect>(x, _effectParent, x => x.Initialize())).ToArray();
            _battleEffectPools = _battleEffectPrefabs.ToDictionary(x => (x.Color, x.IsLarge), x => new ObjectPool<FrameEffect>(x.Prefab, _effectParent, x => x.Initialize()));
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
                    obj.Play(position, Vector3.zero, Vector3.zero, (_screenUpperLeft, _screenLowerRight), false, disposable);
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
                    obj.Play(position, new Vector3(15f, 5f, 0f), new Vector3(0f, 0f, 0f), (_screenUpperLeft, _screenLowerRight), true, disposable);
                    break;
            }
        }
    }
}