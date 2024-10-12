using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rhythm
{
    public class EffectObject : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameTime;

        private SpriteRenderer _spriteRenderer;
        private Action _destroyer;
        private Action<Transform, SpriteRenderer, Action> _onPlay;
        private Action<Transform, SpriteRenderer, Action> _onStop;


        private void Awake()
        {
            gameObject.SetActive(false);
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Create(IDisposable disposable)
        {
            Create(null, null, disposable);
        }

        public void Create(Action<Transform, SpriteRenderer, Action> onPlay, Action<Transform, SpriteRenderer, Action> onStop, IDisposable disposable)
        {
            _onPlay = onPlay;
            _onStop = onStop;

            _destroyer = () =>
            {
                gameObject.SetActive(false);
                disposable.Dispose();
            };
        }

        public void Play(Vector3 position, Action<Transform, SpriteRenderer, Action> onPlay = null)
        {
            transform.localPosition = position;
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(transform, _spriteRenderer, () => Stop());
            }
            else
            {
                _onPlay?.Invoke(transform, _spriteRenderer, () => Stop());
            }
        }

        public void PlayAnimation(Vector3 position, bool isLoop = false, bool isKeep = false, Action<Transform, SpriteRenderer, Action> onPlay = null)
        {
            IEnumerator Effect()
            {
                do
                {
                    foreach (var sprite in _sprites)
                    {
                        if (_spriteRenderer != null) _spriteRenderer.sprite = sprite;
                        yield return new WaitForSeconds(_frameTime);
                    }
                } while (isLoop);
                if (!isKeep) Stop();
            }

            transform.localPosition = position;
            if (_spriteRenderer != null) _spriteRenderer.sprite = _sprites.FirstOrDefault();
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(transform, _spriteRenderer, () => Stop());
            }
            else
            {
                _onPlay?.Invoke(transform, _spriteRenderer, () => Stop());
            }

            StartCoroutine(Effect());
        }

        public void PlayAnimation(Vector3 position, Vector3 direction, bool isLoop = false, bool isKeep = false, Action<Transform, SpriteRenderer, Action> onPlay = null)
        {
            IEnumerator Effect()
            {
                do
                {
                    foreach (var sprite in _sprites)
                    {
                        if (_spriteRenderer != null) _spriteRenderer.sprite = sprite;
                        yield return new WaitForSeconds(_frameTime);
                    }
                } while (isLoop);
                if (!isKeep) Stop();
            }

            transform.localPosition = position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction - position);
            if (_spriteRenderer != null) _spriteRenderer.sprite = _sprites.FirstOrDefault();                         
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(transform, _spriteRenderer, () => Stop());
            }
            else
            {
                _onPlay?.Invoke(transform, _spriteRenderer, () => Stop());
            }

            StartCoroutine(Effect());
        }

        public void Stop(Action<Transform, SpriteRenderer, Action> onStop = null)
        {
            if (onStop is not null)
            {
                onStop.Invoke(transform, _spriteRenderer, _destroyer);
            }
            else
            {
                if (_onStop is not null)
                {
                    _onStop.Invoke(transform, _spriteRenderer, _destroyer);
                }
                else
                {
                    _destroyer?.Invoke();
                }
            }
        }
    }
}