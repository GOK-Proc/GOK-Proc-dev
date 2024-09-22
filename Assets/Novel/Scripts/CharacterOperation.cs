using DG.Tweening;
using Novel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Novel
{
    public class CharacterOperation : MonoBehaviour
    {
        public Dictionary<string, CharacterMatarial> CharacterMaterialDict { get; set; }
        [SerializeField] private Transform _characterParent;
        [SerializeField] private RectTransform _mainCanvasTransform;
        [SerializeField] private float _margin;
        private float _width;

        private Dictionary<string, CharacterState> _preCharacter = new Dictionary<string, CharacterState>();

        private void Start()
        {
            _width = _mainCanvasTransform.sizeDelta.x;
        }

        public void UpdateCharacterLayout(CharacterLayoutData characterLayoutData)
        {
            float space = (_width - _margin * 2) / (characterLayoutData.Layout.Count + 1);

            var sequence = DOTween.Sequence();

            Dictionary<string, CharacterState> currentCharacter = new Dictionary<string, CharacterState>();

            // 処理が終わるまで進まないようにする
            if (characterLayoutData.Motion[0] != "Cut")
            {
                NovelManager.Instance.IsProcessingCharacter = true;
            }

            for (int i = 0; i < characterLayoutData.Layout.Count; i++)
            {
                string character = characterLayoutData.Layout[i];

                // Noneの場合はなにもしない(_preCharacterにも追加されない)
                if (character == "None")
                {
                    continue;
                }

                // キャラクターの移動処理(配置が変化していないキャラも処理の対象)
                if (_preCharacter.ContainsKey(character))
                {
                    CharacterState characterState = _preCharacter[character];
                    currentCharacter[character] = characterState;

                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            characterState.Parent.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            break;

                        default:
                            sequence.Join(characterState.Parent.transform.DOLocalMove(new Vector2(-_width / 2 + _margin + (i + 1) * space, 0), NovelManager.Instance.Duration).SetEase(Ease.InOutQuad));
                            break;
                    }
                }
                // キャラクターの登場処理
                else
                {
                    CharacterState characterState = new CharacterState(character, _characterParent);
                    characterState.ChangeDifference(CharacterMaterialDict[character].Default.name, Instantiate(CharacterMaterialDict[character].Default, characterState.Parent.transform));
                    currentCharacter[character] = characterState;
                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            characterState.Parent.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            break;

                        case "Enter":
                            switch (characterLayoutData.Motion[1])
                            {
                                case "right":
                                    characterState.Parent.transform.localPosition = new Vector2(_width, 0);
                                    break;
                                case "left":
                                    characterState.Parent.transform.localPosition = new Vector2(-_width, 0);
                                    break;
                                default:
                                    throw new Exception("キャラモーション\"Enter\"の方向が正しく指定されていません。");
                            }

                            sequence.Join(characterState.Parent.transform.DOLocalMove(new Vector2(-_width / 2 + _margin + (i + 1) * space, 0), NovelManager.Instance.Duration));
                            break;

                        default:
                            characterState.Parent.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            Image characterImage = characterState.CharacterObject.GetComponent<Image>();
                            SetTransparent(characterImage);

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
                    CharacterState characterState = _preCharacter[character];

                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            characterState.DestroyCharacter();
                            break;
                        case "Leave":
                            switch (characterLayoutData.Motion[1])
                            {
                                case "right":
                                    sequence.Join(characterState.Parent.transform.DOLocalMove(new Vector2(_width, 0), NovelManager.Instance.Duration).SetEase(Ease.InQuad).OnComplete(() =>
                                    {
                                        characterState.DestroyCharacter();
                                    }));
                                    break;
                                case "left":
                                    sequence.Join(characterState.Parent.transform.DOLocalMove(new Vector2(-_width, 0), NovelManager.Instance.Duration).SetEase(Ease.InQuad).OnComplete(() =>
                                    {
                                        characterState.DestroyCharacter();
                                    }));
                                    break;
                                default:
                                    throw new Exception("キャラモーション\"Leave\"の方向が正しく指定されていません。");
                            }
                            break;
                        default:
                            sequence.Join(characterState.CharacterObject.GetComponent<Image>().DOFade(0f, NovelManager.Instance.Duration).OnComplete(() =>
                            {
                                characterState.DestroyCharacter();
                                Debug.Log("end");
                            }));
                            break;
                    }
                }
            }

            // _preCharacterの更新
            _preCharacter = currentCharacter;

            sequence.Play().OnComplete(() =>
            {
                NovelManager.Instance.IsProcessingCharacter = false;
            });
        }

        private void SetTransparent(Image image)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }

        private class CharacterState
        {
            public GameObject Parent { get; private set; }
            public string Difference { get; private set; }
            public GameObject CharacterObject { get; private set; }

            public CharacterState(string name, Transform characterParent)
            {
                Parent = new GameObject(name);
                Parent.AddComponent<RectTransform>();
                Parent.transform.SetParent(characterParent, false);
            }

            public void ChangeDifference(string difference, GameObject characterObject)
            {
                Difference = difference;

                if (CharacterObject != null)
                {
                    DestroyCharacterChildren();
                    Destroy(CharacterObject);
                }

                CharacterObject = characterObject;
            }

            public void DestroyCharacter()
            {
                Destroy(Parent);
                Difference = null;
                DestroyCharacterChildren();
                Destroy(CharacterObject);
            }

            private void DestroyCharacterChildren()
            {
                foreach (Transform child in CharacterObject.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
