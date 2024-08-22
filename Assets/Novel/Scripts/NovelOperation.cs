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
        private void Start()
        {
            _width = _mainCanvasTransform.sizeDelta.x;
        }

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;

        public void UpdateDialogue(DialogueData dialogueData)
        {
            string name = dialogueData.Name;
            string dialogue = dialogueData.Dialogue;

            _nameText.text = name;
            _dialogueText.text = dialogue;
        }

        public Dictionary<string, GameObject> CharacterPrefabs { get; set; }
        [SerializeField] private GameObject _characterParent;
        [SerializeField] private RectTransform _mainCanvasTransform;
        [SerializeField] private float _margin;
        private float _width;

        //private List<string> _preCharacterLayout = new List<string>();
        private Dictionary<string, GameObject> _preCharacter = new Dictionary<string, GameObject>();

        public void UpdateCharacterLayout(CharacterLayoutData characterLayoutData)
        {
            // キャラクターがどう変化するかの辞書
            Dictionary<string, CharacterTransition> characterTransitionDict = new Dictionary<string, CharacterTransition>();

            foreach (string character in characterLayoutData.CharacterLayout)
            {
                if (_preCharacter.ContainsKey(character))
                {
                    characterTransitionDict[character] = CharacterTransition.Remain;
                }
                else
                {
                    characterTransitionDict[character] = CharacterTransition.Appear;
                }
            }

            foreach (string character in _preCharacter.Keys)
            {
                if (!characterLayoutData.CharacterLayout.Contains(character))
                {
                    characterTransitionDict[character] = CharacterTransition.Disappear;
                }
            }

            float space = _width / (characterLayoutData.CharacterLayout.Count + 1);

            var sequence = DOTween.Sequence();

            for (int i = 0; i < characterLayoutData.CharacterLayout.Count; i++)
            {
                string character = characterLayoutData.CharacterLayout[i];
                switch (characterTransitionDict[character])
                {
                    case CharacterTransition.Remain:
                        // dotween使って動かす
                        break;

                    case CharacterTransition.Appear:
                        //GameObject characterObject = Instantiate();
                        //sequence.Join(characterObject.DOFade)
                        break;

                    default:
                        throw new Exception("CharacterTransition Type is not appropriate!");

                }
            }
        }

        enum CharacterTransition
        {
            Remain,
            Appear,
            Disappear
        }


        [SerializeField] private Image _backgroundImage;
        
        public void UpdateBackground(BackgroundData backgroundData)
        {
            
        }


        public void UpdateBgm(BgmData bgmData)
        {

        }


        public void ExecuteOtherOperation(OtherData otherData)
        {

        }
    }
}
