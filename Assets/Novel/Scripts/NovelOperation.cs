using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

namespace Novel
{
    public class NovelOperation : MonoBehaviour
    {
        private float _duration = 0.75f;

        private void Start()
        {
            _replaceDictionary = new ReplaceDictionary();

            _width = _mainCanvasTransform.sizeDelta.x;
        }

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        private ReplaceDictionary _replaceDictionary;

        public void UpdateDialogue(DialogueData dialogueData)
        {
            string name = dialogueData.Name;
            string dialogue = dialogueData.Dialogue;

            name = ReplaceParamater(name);
            dialogue = ReplaceParamater(dialogue);

            _nameText.text = name;
            _dialogueText.text = dialogue;
        }

        private string ReplaceParamater(string sentence)
        {
            int foundIndex = sentence.IndexOf("${");
            if (foundIndex >= 0)
            {
                int nextIndex = sentence.IndexOf("}", foundIndex + 2);
                if (nextIndex >= 0)
                {
                    string target = sentence.Substring(foundIndex + 2, nextIndex - (foundIndex + 2));
                    string replaced = sentence.Replace("${" + target + "}", _replaceDictionary.ReplaceDict[target]);

                    return ReplaceParamater(replaced);
                }
                else
                {
                    throw new Exception("変数置換のための\"${\"に対応する\"}\"がありません。");
                }
            }
            else
            {
                return sentence;
            }
        }

        public Dictionary<string, GameObject> CharacterPrefabDict { get; set; } = new Dictionary<string, GameObject>();
        [SerializeField] private Transform _characterParent;
        [SerializeField] private RectTransform _mainCanvasTransform;
        [SerializeField] private float _margin;
        private float _width;

        private Dictionary<string, GameObject> _preCharacter = new Dictionary<string, GameObject>();

        public void UpdateCharacterLayout(CharacterLayoutData characterLayoutData)
        {
            float space = (_width - _margin * 2) / (characterLayoutData.CharacterLayout.Count + 1);

            var sequence = DOTween.Sequence();

            Dictionary<string, GameObject> currentCharacter = new Dictionary<string, GameObject>();

            for (int i = 0; i < characterLayoutData.CharacterLayout.Count; i++)
            {
                string character = characterLayoutData.CharacterLayout[i];

                if (_preCharacter.ContainsKey(character))
                {
                    GameObject characterObject = _preCharacter[character];
                    currentCharacter[character] = characterObject;

                    sequence.Join(characterObject.transform.DOLocalMove(new Vector2(- _width / 2 + _margin + (i + 1) * space, 0), _duration));
                }
                else
                {
                    GameObject characterObject = Instantiate(CharacterPrefabDict[character], _characterParent);
                    currentCharacter[character] = characterObject;
                    characterObject.transform.localPosition = new Vector2(-_width / 2 + _margin + (i + 1) * space, 0);
                    Image characterImage = characterObject.GetComponent<Image>();
                    Color color = characterImage.color;
                    color.a = 0f;
                    characterImage.color = color;

                    sequence.Join(characterImage.DOFade(1f, _duration));
                }
            }

            List<GameObject> exitCharacter = new List<GameObject>();

            foreach (string character in _preCharacter.Keys)
            {
                if (!characterLayoutData.CharacterLayout.Contains(character))
                {
                    GameObject characterObject = _preCharacter[character];
                    exitCharacter.Add(characterObject);
                    sequence.Join(characterObject.GetComponent<Image>().DOFade(0f, _duration));
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
            });
        }


        public Dictionary<string, Sprite> BackgroundImageDict { get; set; } = new Dictionary<string, Sprite>();
        [SerializeField] private Image _backgroundImage;
        
        public void UpdateBackground(BackgroundData backgroundData)
        {
            if (_backgroundImage.sprite != null)
            {
                switch (backgroundData.Motion)
                {
                    case "Fade":
                        _backgroundImage.DOFade(0f, _duration).OnComplete(() =>
                        {
                            _backgroundImage.sprite = BackgroundImageDict[backgroundData.Background];

                            _backgroundImage.DOFade(1f, _duration);
                        });
                        break;

                    default:
                        throw new Exception("背景の変化方法が正しく指定されていません。");
                }
            }
            else
            {
                switch (backgroundData.Motion)
                {
                    case "Fade":
                        _backgroundImage.sprite = BackgroundImageDict[backgroundData.Background];
                        Color color = _backgroundImage.color;
                        color.a = 0f;
                        _backgroundImage.color = color;

                        _backgroundImage.DOFade(1f, _duration);
                        break;

                    default:
                        throw new Exception("背景の変化方法が正しく指定されていません。");
                }
            }
        }


        public void UpdateBgm(BgmData bgmData)
        {

        }


        public void ExecuteOtherOperation(OtherData otherData)
        {

        }
    }
}
