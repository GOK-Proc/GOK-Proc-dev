using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Novel
{
    public class NovelOperation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _backgroundImage;

        [SerializeField] private GameObject _characterParent;

        public void UpdateDialogue(DialogueData dialogueData)
        {
            string name = dialogueData.Name;
            string dialogue = dialogueData.Dialogue;

            _nameText.text = name;
            _dialogueText.text = dialogue;
        }

        public void UpdateCharacterLayout(CharacterLayoutData characterLayout)
        {

        }

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
