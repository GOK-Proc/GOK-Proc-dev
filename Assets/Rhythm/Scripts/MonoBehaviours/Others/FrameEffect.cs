using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rhythm
{
    public class FrameEffect : RhythmGameObject
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameTime;

        private SpriteRenderer _spriteRenderer;

        public void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayEffect(Vector3 position, IDisposable disposable)
        {

            IEnumerator Effect(Action callBack)
            {
                foreach (var sprite in _sprites)
                {
                    _spriteRenderer.sprite = sprite;
                    yield return new WaitForSeconds(_frameTime);
                }
                callBack?.Invoke();
            }

            _spriteRenderer.sprite = _sprites.FirstOrDefault();
            Create(position, Vector3.zero, (new Vector2(-9.6f, 5.4f), new Vector2(9.6f, -5.4f)), disposable);
            StartCoroutine(Effect(_onDestroy));
        }
    }
}