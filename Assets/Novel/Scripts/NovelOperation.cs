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

        public void UpdateDialogue(params object[] args)
        {
            Debug.Log(args.Length);

            if (args.Length != 2)
            {
                throw new Exception("The number of paramaters is incorrect.");
            }

            string name = args[0].ToString();
            string dialogue = args[1].ToString();

            _nameText.text = name;
            _dialogueText.text = dialogue;
        }
    }
}
