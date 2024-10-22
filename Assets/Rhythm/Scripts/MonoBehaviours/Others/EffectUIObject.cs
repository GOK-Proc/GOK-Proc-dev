using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Rhythm
{
    public class EffectUIObject : MonoBehaviour
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _frameTime;

        private RectTransform _rectTransform;
        private Image _image;
        private TextMeshProUGUI _textMeshProUGUI;
        private Action _destroyer;
        private Action<RectTransform, Image, Action> _onPlayImage;
        private Action<RectTransform, Image, Action> _onStopImage;
        private Action<RectTransform, TextMeshProUGUI, Action> _onPlayText;
        private Action<RectTransform, TextMeshProUGUI, Action> _onStopText;


        private void Awake()
        {
            gameObject.SetActive(false);
            _rectTransform = GetComponent<RectTransform>();
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _image = GetComponent<Image>();
        }

        public void Create(IDisposable disposable)
        {
            CreateImage(null, null, disposable);
        }

        public void CreateImage(Action<RectTransform, Image, Action> onPlay, Action<RectTransform, Image, Action> onStop, IDisposable disposable)
        {
            _onPlayImage = onPlay;
            _onStopImage = onStop;

            _destroyer = () =>
            {
                gameObject.SetActive(false);
                disposable.Dispose();
            };
        }

        public void CreateText(Action<RectTransform, TextMeshProUGUI, Action> onPlay, Action<RectTransform, TextMeshProUGUI, Action> onStop, IDisposable disposable)
        {
            _onPlayText = onPlay;
            _onStopText = onStop;

            _destroyer = () =>
            {
                gameObject.SetActive(false);
                disposable.Dispose();
            };
        }

        public void PlayImage(Vector2 anchoredPosition, Action<RectTransform, Image, Action> onPlay = null)
        {
            _rectTransform.anchoredPosition = anchoredPosition;
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(_rectTransform, _image, () => StopImage());
            }
            else
            {
                _onPlayImage?.Invoke(_rectTransform, _image, () => StopImage());
            }
        }

        public void PlayText(Vector2 anchoredPosition, Action<RectTransform, TextMeshProUGUI, Action> onPlay = null)
        {
            _rectTransform.anchoredPosition = anchoredPosition;
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(_rectTransform, _textMeshProUGUI, () => StopText());
            }
            else
            {
                _onPlayText?.Invoke(_rectTransform, _textMeshProUGUI, () => StopText());
            }
        }

        public void PlayAnimation(Vector2 anchoredPosition, bool isLoop = false, Action<RectTransform, Image, Action> onPlay = null)
        {
            IEnumerator Effect()
            {
                do
                {
                    foreach (var sprite in _sprites)
                    {
                        if (_image != null) _image.sprite = sprite;
                        yield return new WaitForSeconds(_frameTime);
                    }
                } while (isLoop);
                StopImage();
            }

            _rectTransform.anchoredPosition = anchoredPosition;
            if (_image != null) _image.sprite = _sprites.FirstOrDefault();
            gameObject.SetActive(true);

            if (onPlay is not null)
            {
                onPlay.Invoke(_rectTransform, _image, () => StopImage());
            }
            else
            {
                _onPlayImage?.Invoke(_rectTransform, _image, () => StopImage());
            }

            StartCoroutine(Effect());
        }

        public void StopImage(Action<RectTransform, Image, Action> onStop = null)
        {
            if (onStop is not null)
            {
                onStop.Invoke(_rectTransform, _image, _destroyer);
            }
            else
            {
                if (_onStopImage is not null)
                {
                    _onStopImage.Invoke(_rectTransform, _image, _destroyer);
                }
                else
                {
                    _destroyer?.Invoke();
                }
            }
        }

        public void StopText(Action<RectTransform, TextMeshProUGUI, Action> onStop = null)
        {
            if (onStop is not null)
            {
                onStop.Invoke(_rectTransform, _textMeshProUGUI, _destroyer);
            }
            else
            {
                if (_onStopText is not null)
                {
                    _onStopText.Invoke(_rectTransform, _textMeshProUGUI, _destroyer);
                }
                else
                {
                    _destroyer?.Invoke();
                }
            }
        }
    }
}