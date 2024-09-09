using DG.Tweening;
using Novel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Novel
{
    public class CharacterOperation : MonoBehaviour
    {
        public Dictionary<string, GameObject> CharacterPrefabDict { get; set; } = new Dictionary<string, GameObject>();
        [SerializeField] private Transform _characterParent;
        [SerializeField] private RectTransform _mainCanvasTransform;
        [SerializeField] private float _margin;
        private float _width;

        private Dictionary<string, GameObject> _preCharacter = new Dictionary<string, GameObject>();

        private void Start()
        {
            _width = _mainCanvasTransform.sizeDelta.x;
        }

        public void UpdateCharacterLayout(CharacterLayoutData characterLayoutData)
        {
            float space = (_width - _margin * 2) / (characterLayoutData.Layout.Count + 1);

            var sequence = DOTween.Sequence();

            Dictionary<string, GameObject> currentCharacter = new Dictionary<string, GameObject>();

            if (characterLayoutData.Motion[0] != "Cut")
            {
                NovelManager.Instance.IsProcessingCharacter = true;
            }

            for (int i = 0; i < characterLayoutData.Layout.Count; i++)
            {
                string character = characterLayoutData.Layout[i];

                // キャラクターの移動処理
                if (_preCharacter.ContainsKey(character))
                {
                    GameObject characterObject = _preCharacter[character];
                    currentCharacter[character] = characterObject;

                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            characterObject.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            break;

                        default:
                            sequence.Join(characterObject.transform.DOLocalMove(new Vector2(-_width / 2 + _margin + (i + 1) * space, 0), NovelManager.Instance.Duration).SetEase(Ease.InOutQuad));
                            break;
                    }
                }
                // キャラクターの登場処理
                else
                {
                    GameObject characterObject = Instantiate(CharacterPrefabDict[character], _characterParent);
                    currentCharacter[character] = characterObject;
                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            characterObject.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            break;

                        case "Enter":
                            switch (characterLayoutData.Motion[1])
                            {
                                case "right":
                                    characterObject.transform.localPosition = new Vector2(_width, 0);
                                    break;
                                case "left":
                                    characterObject.transform.localPosition = new Vector2(-_width, 0);
                                    break;
                                default:
                                    throw new Exception("キャラモーション\"Enter\"の方向が正しく指定されていません。");
                            }

                            sequence.Join(characterObject.transform.DOLocalMove(new Vector2(-_width / 2 + _margin + (i + 1) * space, 0), NovelManager.Instance.Duration));
                            break;

                        default:
                            characterObject.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            Image characterImage = characterObject.GetComponent<Image>();
                            Color color = characterImage.color;
                            color.a = 0f;
                            characterImage.color = color;

                            sequence.Join(characterImage.DOFade(1f, NovelManager.Instance.Duration));
                            break;
                    }
                }
            }

            List<GameObject> exitCharacter = new List<GameObject>();

            // キャラクターの退場処理
            foreach (string character in _preCharacter.Keys)
            {
                if (!characterLayoutData.Layout.Contains(character))
                {
                    GameObject characterObject = _preCharacter[character];

                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            Destroy(characterObject);
                            break;
                        case "Leave":
                            exitCharacter.Add(characterObject);

                            switch (characterLayoutData.Motion[1])
                            {
                                case "right":
                                    sequence.Join(characterObject.transform.DOLocalMove(new Vector2(_width, 0), NovelManager.Instance.Duration).SetEase(Ease.InQuad));
                                    break;
                                case "left":
                                    sequence.Join(characterObject.transform.DOLocalMove(new Vector2(-_width, 0), NovelManager.Instance.Duration).SetEase(Ease.InQuad));
                                    break;
                                default:
                                    throw new Exception("キャラモーション\"Leave\"の方向が正しく指定されていません。");
                            }

                            break;
                        default:
                            exitCharacter.Add(characterObject);
                            sequence.Join(characterObject.GetComponent<Image>().DOFade(0f, NovelManager.Instance.Duration));
                            break;
                    }
                }
            }

            // _preCharacterの更新
            _preCharacter = currentCharacter;

            sequence.Play().OnComplete(() =>
            {
                foreach (GameObject characterObject in exitCharacter)
                {
                    Destroy(characterObject);
                }

                NovelManager.Instance.IsProcessingCharacter = false;
            });
        }
    }
}
