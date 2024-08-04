using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class FrameEffect : AccelerateObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameTime;

        private SpriteRenderer _spriteRenderer;

        public void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Play(Vector3 position, Vector3 velocity, Vector3 acceleration, (Vector2 UpperLeft, Vector2 LowerRight) survivalRect, bool isLoop, IDisposable disposable)
        {

            IEnumerator Effect(Action callBack)
            {
                do
                {
                    foreach (var sprite in _sprites)
                    {
                        _spriteRenderer.sprite = sprite;
                        yield return new WaitForSeconds(_frameTime);
                    }
                } while (isLoop);
                callBack?.Invoke();
            }

            _spriteRenderer.sprite = _sprites.FirstOrDefault();
            Create(position, velocity, acceleration, survivalRect, disposable);
            StartCoroutine(Effect(_onDestroy));
        }
    }
}