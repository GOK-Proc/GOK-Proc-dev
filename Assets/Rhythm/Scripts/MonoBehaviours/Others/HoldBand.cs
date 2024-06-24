using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class HoldBand : LaneObject
    {
        private double _beginTime;
        private double _endTime;

        private Color _defaultColor;
        private Color _pressedColor;
        private Color _releasedColor;

        private Transform _mask;
        private SpriteRenderer _renderer;

        private ITimeProvider _timeProvider;
        private IColorInputProvider _colorInputProvider;

        public void Initialize(ITimeProvider timeProvider, IColorInputProvider colorInputProvider)
        {
            _timeProvider = timeProvider;
            _colorInputProvider = colorInputProvider;

            _renderer = GetComponent<SpriteRenderer>();
            _defaultColor = _renderer.color;

            _pressedColor = _renderer.color;
            _pressedColor.a = 0.9f;

            _releasedColor = _renderer.color;
            _releasedColor.a = 0.5f;
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, float length, double beginTime, double endTime, Transform mask, IDisposable disposable)
        {
            transform.localScale = new Vector3(transform.localScale.x, length, transform.localScale.z);
            _beginTime = beginTime;
            _endTime = endTime;
            _mask = mask;
            _renderer.color = _defaultColor;

            Create(position, velocity, survivalRect, lane, disposable);

            _onDestroy = () =>
            {
                IsAlive = false;
                gameObject.SetActive(false);
                _mask.gameObject.SetActive(false);
                disposable.Dispose();
            };
        }

        protected override void Update()
        {
            base.Update();

            var time = _timeProvider.Time;

            if (time >= _beginTime && time <= _endTime)
            {
                if (_colorInputProvider.IsColorPressed(_color))
                {
                    _mask.gameObject.SetActive(true);
                    _renderer.color = _pressedColor;
                }
                else
                {
                    _mask.gameObject.SetActive(false);
                    _renderer.color = _releasedColor;
                }
            }
            
        }
    }
}