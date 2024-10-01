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
        public Dictionary<string, GameObject> BackgroundImageDict { get; set; }

        [SerializeField] private Transform _backgroundParent;
        [SerializeField] private GameObject _namePanel;
        [SerializeField] private GameObject _dialoguePanel;

        private GameObject _preBackground;


        public void UpdateBackground(BackgroundData backgroundData)
        {
            GameObject backgroundObject= null;

            var sequence = DOTween.Sequence();

            if (backgroundData.Motion != "Cut")
            {
                NovelManager.Instance.IsProcessingBackground = true;
            }


            switch (backgroundData.Motion)
            {
                case "Fade":
                    // もともと画像があった場合
                    if (_preBackground != null)
                    {
                        if (backgroundData.Background == "Blackout")
                        {
                            sequence.Join(_preBackground.GetComponent<Image>().DOFade(0f, NovelManager.Instance.Duration).SetEase(Ease.Linear));

                            // 枠を非表示にする
                            _namePanel.SetActive(false);
                            _dialoguePanel.SetActive(false);
                        }
                        else
                        {
                            sequence.Join(_preBackground.GetComponent<Image>().DOFade(0f, NovelManager.Instance.Duration).SetEase(Ease.Linear)).OnComplete(() =>
                            {
                                Destroy(_preBackground);
                            });

                            backgroundObject = Instantiate(BackgroundImageDict[backgroundData.Background], _backgroundParent);
                            Image backgroundImage = backgroundObject.GetComponent<Image>();
                            SetTransparent(backgroundImage);

                            sequence.Join(backgroundImage.DOFade(1f, NovelManager.Instance.Duration).SetEase(Ease.Linear));
                        }
                    }
                    // もともと画像がなかった場合
                    else
                    {
                        if (backgroundData.Background != "Blackout")
                        {
                            backgroundObject = Instantiate(BackgroundImageDict[backgroundData.Background], _backgroundParent);
                            Image backgroundImage = backgroundObject.GetComponent<Image>();
                            SetTransparent(backgroundImage);

                            sequence.Join(backgroundImage.DOFade(1f, NovelManager.Instance.Duration).SetEase(Ease.Linear).OnComplete(() =>
                            {
                                // 枠を表示する
                                _namePanel.SetActive(true);
                                _dialoguePanel.SetActive(true);
                            }));
                        }
                    }
                    break;

                case "Cut":
                    if (backgroundData.Background != "Blackout")
                    {
                        Destroy(_preBackground);
                        backgroundObject = Instantiate(BackgroundImageDict[backgroundData.Background], _backgroundParent);

                        // 枠を表示する
                        _namePanel.SetActive(true);
                        _dialoguePanel.SetActive(true);
                    }
                    else
                    {
                        Destroy(_preBackground);

                        // 枠を非表示にする
                        _namePanel.SetActive(false);
                        _dialoguePanel.SetActive(false);
                    }
                    break;

                default:
                    throw new Exception("背景の変化方法が正しく指定されていません。");
            }

            _preBackground = backgroundObject;

            sequence.Play().OnComplete(() =>
            {
                NovelManager.Instance.IsProcessingBackground = false;
            });
        }

        private void SetTransparent(Image image)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }
    }
}
