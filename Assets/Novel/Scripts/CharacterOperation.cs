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
        private float _shortDuration = 0.4f;

        private Dictionary<string, CharacterState> _preCharacter = new Dictionary<string, CharacterState>();

        private bool _stopHighlight = false;

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
                // Noneの場合はなにもしない(_preCharacterにも追加されない)
                if (characterLayoutData.Layout[i] == "None")
                {
                    continue;
                }

                string[] arguments = characterLayoutData.Layout[i].Split('.');
                string character = arguments[0];
                string difference;

                CharacterMatarial characterMatarial = CharacterMaterialDict[character];

                // 差分を指定
                if (arguments.Length == 2)
                {
                    difference = arguments[1];
                }
                else
                {
                    difference = CharacterMaterialDict[character].Default.name;
                }

                // キャラクターの移動処理(配置が変化していないキャラも処理の対象)
                if (_preCharacter.ContainsKey(character))
                {
                    CharacterState characterState = _preCharacter[character];
                    currentCharacter[character] = characterState;

                    switch (characterLayoutData.Motion[0])
                    {
                        case "Cut":
                            // 差分の変更
                            if (characterState.Difference != difference)
                            {
                                GameObject differenceObject = Instantiate(characterMatarial.CharacterDifferenceDict[difference], characterState.Parent.transform);
                                characterState.ChangeDifference(difference, differenceObject);
                            }

                            // 移動
                            characterState.Parent.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                            break;

                        default:
                            // 差分の変更
                            if (characterState.Difference != difference)
                            {
                                // 新たな差分のインスタンス化とフェードイン
                                GameObject differenceObject = Instantiate(characterMatarial.CharacterDifferenceDict[difference], characterState.Parent.transform);
                                Image characterImage = differenceObject.GetComponent<Image>();

                                // ハイライト中でないなら、変更後の差分も非ハイライト状態で登場させる
                                if (!characterState.IsHighlighted)
                                {
                                    characterImage.color = Color.gray;
                                }

                                SetTransparent(characterImage);
                                sequence.Join(characterImage.DOFade(1f, _shortDuration));

                                // 前の差分のフェードアウト
                                sequence.Join(characterState.CharacterObject.GetComponent<Image>().DOFade(0f, _shortDuration).OnComplete(() =>
                                {
                                    characterState.ChangeDifference(difference, differenceObject);
                                }));
                            }

                            // 移動
                            sequence.Join(characterState.Parent.transform.DOLocalMove(new Vector2(-_width / 2 + _margin + (i + 1) * space, 0), NovelManager.Instance.Duration).SetEase(Ease.InOutQuad));
                            break;
                    }
                }
                // キャラクターの登場処理
                else
                {
                    CharacterState characterState = new CharacterState(character, _characterParent);
                    GameObject differenceObject = Instantiate(characterMatarial.CharacterDifferenceDict[difference], characterState.Parent.transform);
                    characterState.ChangeDifference(difference, differenceObject);
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

            // キャラクターの退場処理
            foreach (string character in _preCharacter.Keys)
            {
                if (!currentCharacter.ContainsKey(character))
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
                            }));
                            break;
                    }
                }
            }

            // _preCharacterの更新
            _preCharacter = currentCharacter;

            sequence.Play().OnComplete(() =>
            {
                _stopHighlight = false;
                NovelManager.Instance.IsProcessingCharacter = false;
            });
        }

        private void SetTransparent(Image image)
        {
            Color color = image.color;
            color.a = 0f;
            image.color = color;
        }

        public void UpdateCharacterHighlight(HighlightData highlightData)
        {
            _stopHighlight = highlightData.Wait;

            StartCoroutine(HighlightCoroutine(highlightData.Highlight, highlightData.AllHighlight));
        }

        private IEnumerator HighlightCoroutine(List<string> highlight, bool allHighlight)
        {
            yield return new WaitWhile(() => _stopHighlight);

            // ハイライトするキャラクターが指定されている場合
            if (highlight[0] != "")
            {
                foreach (string character in _preCharacter.Keys)
                {
                    _preCharacter[character].SetHighlight(highlight.Contains(character));
                }
            }
            else
            {
                foreach (string character in _preCharacter.Keys)
                {
                    _preCharacter[character].SetHighlight(allHighlight);
                }
            }
        }

        private class CharacterState
        {
            public GameObject Parent { get; private set; }
            public string Difference { get; private set; }
            public GameObject CharacterObject { get; private set; }
            public bool IsHighlighted { get; private set;  } = true;

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

            public void SetHighlight(bool isHighlight)
            {
                if (IsHighlighted && !isHighlight)
                {
                    Image characterImage = CharacterObject.GetComponent<Image>();
                    characterImage.color = Color.gray;
                    IsHighlighted = false;
                }
                else if (!IsHighlighted && isHighlight)
                {
                    Image characterImage = CharacterObject.GetComponent<Image>();
                    characterImage.color = Color.white;
                    IsHighlighted = true;
                }
            }
        }
    }
}
