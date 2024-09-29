using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rhythm
{
    public class IntroSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _introSource;
        public AudioSource IntroSource => _introSource;

        [SerializeField] private AudioSource _mainSource;
        public AudioSource MainSource => _mainSource;

        private float _volume;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                _introSource.volume = _volume;
                _mainSource.volume = _volume;
            }
        }

        private Coroutine _coroutine;

        public void Play()
        {
            IEnumerator PlayIntroSound()
            {
                _introSource.Play();

                while (_introSource.isPlaying)
                {
                    yield return null;
                }

                _mainSource.Play();
            }

            Stop();
            _coroutine = StartCoroutine(PlayIntroSound());
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _introSource.Stop();
            _mainSource.Stop();
        }

        public void FadeOut(float duration)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(_introSource.DOFade(0f, duration).SetEase(Ease.Linear));
            sequence.Join(_mainSource.DOFade(0f, duration).SetEase(Ease.Linear));

            sequence.Play().OnComplete(() =>
            {
                Stop();
                Volume = _volume;
            });
        }
    }
}