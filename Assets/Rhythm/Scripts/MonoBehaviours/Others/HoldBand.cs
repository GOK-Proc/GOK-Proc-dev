using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rhythm
{
    public class HoldBand : LaneObject
    {
        private double _beginTime;
        private double _endTime;

        private Color _defaultColor;
        private Color _pressedColor;
        private Color _releasedColor;

        private SpriteRenderer _renderer;

        private float _judgeLineY;
        private Vector3 _holdPosition;
        private ITimeProvider _timeProvider;
        private IColorInputProvider _colorInputProvider;
        private ISoundPlayable _soundPlayable;

        private Vector3 _defaultScale;
        private Vector3 _currentScale;
        private bool _isSePlayed;

        private readonly float _fadeOutDuration = 0.3f;

        public void Initialize(float judgeLineY, ITimeProvider timeProvider, IColorInputProvider colorInputProvider, ISoundPlayable soundPlayable)
        {
            _judgeLineY = judgeLineY;
            _timeProvider = timeProvider;
            _colorInputProvider = colorInputProvider;
            _soundPlayable = soundPlayable;

            _renderer = GetComponent<SpriteRenderer>();
            _defaultColor = _renderer.color;

            _pressedColor = _renderer.color;
            _pressedColor.a = 0.9f;

            _releasedColor = _renderer.color;
            _releasedColor.a = 0.5f;
        }

        public void Create(Vector3 position, Vector3 velocity, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, int lane, float length, double beginTime, double endTime, IDisposable disposable)
        {
            _defaultScale = new Vector3 (transform.localScale.x, length, transform.localScale.z);
            _currentScale = _defaultScale;
            transform.localScale = _defaultScale;
            _beginTime = beginTime;
            _endTime = endTime;
            _holdPosition = new Vector3(position.x, _judgeLineY, position.z);
            _renderer.color = _defaultColor;

            Create(position, velocity, survivalRect, lane, x => 
            {
                if (_isSePlayed)
                {
                    _soundPlayable.FadeOutSE("Hold", _lane, _fadeOutDuration);
                    _isSePlayed = false;
                }
                x?.Invoke();
            }, disposable);

            _isSePlayed = false;
        }

        public new void Destroy()
        {
            base.Destroy();
        }

        protected override void Update()
        {
            base.Update();

            var time = _timeProvider.Time;

            if (time >= _beginTime && time <= _endTime)
            {
                if (_colorInputProvider.IsColorPressed(_color))
                {
                    _currentScale.y = (float)(_endTime - time) * -_velocity.y;
                    transform.localScale = _currentScale;
                    transform.position = _holdPosition;
                    _renderer.color = _pressedColor;

                    if (!_isSePlayed)
                    {
                        _soundPlayable.PlaySE("Hold", _lane);
                        _isSePlayed = true;
                    }
                }
                else
                {
                    transform.localScale = _defaultScale;
                    _renderer.color = _releasedColor;

                    if (_isSePlayed)
                    {
                        _soundPlayable.FadeOutSE("Hold", _lane, _fadeOutDuration);
                        _isSePlayed = false;
                    }
                }
            }
            else if (time > _endTime)
            {
                if (transform.localScale != _defaultScale) gameObject.SetActive(false);
                if (_isSePlayed)
                {
                    _soundPlayable.FadeOutSE("Hold", _lane, _fadeOutDuration);
                    _isSePlayed = false;
                }
            }
        }
    }
}