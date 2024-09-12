using Novel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Novel
{
    public class BackgroundOperation : MonoBehaviour
    {
        public Dictionary<string, Sprite> BackgroundImageDict { get; set; } = new Dictionary<string, Sprite>();
        [SerializeField] private Image _backgroundImage;

        public void UpdateBackground(BackgroundData backgroundData)
        {
            var sequence = DOTween.Sequence();

            if (backgroundData.Motion != "Cut")
            {
                NovelManager.Instance.IsProcessingBackground = true;
            }


            switch (backgroundData.Motion)
            {
                case "Fade":
                    if (_backgroundImage.sprite != null)
                    {
                        if (backgroundData.Background == "Black")
                        {
                            sequence.Join(_backgroundImage.DOFade(0f, NovelManager.Instance.Duration));
                        }
                        else
                        {
                            sequence.Join(_backgroundImage.DOFade(0f, NovelManager.Instance.Duration).OnComplete(() =>
                            {
                                _backgroundImage.sprite = BackgroundImageDict[backgroundData.Background];
                            }));

                            sequence.Append(_backgroundImage.DOFade(1f, NovelManager.Instance.Duration));
                        }
                    }
                    else
                    {
                        if (backgroundData.Background != "Black")
                        {
                            _backgroundImage.sprite = BackgroundImageDict[backgroundData.Background];
                            Color color = _backgroundImage.color;
                            color.a = 0f;
                            _backgroundImage.color = color;

                            sequence.Append(_backgroundImage.DOFade(1f, NovelManager.Instance.Duration));
                        }
                    }
                    break;

                case "Cut":
                    if (backgroundData.Background != "Black")
                    {
                        _backgroundImage.sprite = BackgroundImageDict[backgroundData.Background];
                    }
                    break;

                default:
                    throw new Exception("背景の変化方法が正しく指定されていません。");
            }

            sequence.Play().OnComplete(() =>
            {
                NovelManager.Instance.IsProcessingBackground = false;
            });
        }
    }
}
