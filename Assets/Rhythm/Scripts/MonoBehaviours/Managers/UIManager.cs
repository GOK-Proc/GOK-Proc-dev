using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class UIManager : MonoBehaviour, IUI
    {
        [SerializeField] private Transform _playerGauge;
        [SerializeField] private Transform _enemyGauge;

        private Vector3 _playerGaugePosition;
        private Vector3 _enemyGaugePosition;
        private Vector3 _playerGaugeScale;
        private Vector3 _enemyGaugeScale;

        private void Awake()
        {
            _playerGaugePosition = _playerGauge.position;
            _enemyGaugePosition = _enemyGauge.position;
            _playerGaugeScale = _playerGauge.localScale;
            _enemyGaugeScale = _enemyGauge.localScale;
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
            }
        }
    }
}